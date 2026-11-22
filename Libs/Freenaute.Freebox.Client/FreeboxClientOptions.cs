namespace Freenaute.Freebox.Client;

/// <summary>Configures one Freebox and one application identity.</summary>
public sealed class FreeboxClientOptions : IValidatableObject
{
	/// <summary>The HTTP(S) origin used for discovery and subsequent requests.</summary>
	[Required]
	public Uri ServerAddress { get; set; } = new("https://mafreebox.freebox.fr/");

	/// <summary>An explicit API major override. Leave unset to discover the advertised version.</summary>
	public int? ApiVersion { get; set; }

	/// <summary>The API root path used with an explicit version override.</summary>
	[Required]
	public string ApiBasePath { get; set; } = "/api/";

	/// <summary>The timeout applied by the client to each HTTP operation.</summary>
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>Whether mDNS/DNS-SD is attempted before the HTTP discovery fallback.</summary>
	public bool EnableMdnsDiscovery { get; set; } = true;

	/// <summary>The bounded time allowed for one mDNS/DNS-SD discovery attempt.</summary>
	public TimeSpan MdnsTimeout { get; set; } = TimeSpan.FromSeconds(2);

	/// <summary>The stable application identifier used for pairing and session creation.</summary>
	public string? ApplicationId { get; set; }

	/// <summary>The display name sent during initial physical pairing.</summary>
	[Required]
	public string ApplicationName { get; set; } = "Freenaute Freebox Client";

	/// <summary>The application version sent during pairing and session creation.</summary>
	public string? ApplicationVersion { get; set; }

	/// <summary>The device name shown on the Freebox during pairing.</summary>
	[Required]
	public string DeviceName { get; set; } = "Freenaute";

	/// <summary>A caller-defined credential namespace shared only by instances that should reuse credentials.</summary>
	[Required]
	public string CredentialProfile { get; set; } = "default";

	/// <summary>Whether authenticated requests may lazily open a session with approved stored credentials.</summary>
	public bool AuthenticateAutomatically { get; set; } = true;

	/// <summary>The name used when registering the underlying <see cref="HttpClient" />.</summary>
	[Required]
	public string HttpClientName { get; set; } = FreeboxServiceCollectionExtensions.DefaultHttpClientName;

	/// <summary>Whether to attach the standard resilience handler (opt-in; default no retry on mutations).</summary>
	public bool EnableResilience { get; set; }

	IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
	{
		if (!ServerAddress.IsAbsoluteUri ||
		    (!string.Equals(ServerAddress.Scheme, Uri.UriSchemeHttp, StringComparison.Ordinal) &&
		     !string.Equals(ServerAddress.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal)) ||
		    !string.IsNullOrEmpty(ServerAddress.UserInfo) || !string.IsNullOrEmpty(ServerAddress.Query) ||
		    !string.IsNullOrEmpty(ServerAddress.Fragment) ||
		    !string.Equals(ServerAddress.AbsolutePath, "/", StringComparison.Ordinal))
		{
			yield return new ValidationResult(
				"ServerAddress must be an absolute HTTP(S) origin without a path, credentials, query or fragment.",
				[nameof(ServerAddress)]);
		}

		if (ApiVersion is <= 0)
		{
			yield return new ValidationResult("The API major version must be positive.", [nameof(ApiVersion)]);
		}

		if (Timeout != System.Threading.Timeout.InfiniteTimeSpan &&
		    (Timeout <= TimeSpan.Zero || Timeout.TotalMilliseconds > int.MaxValue))
		{
			yield return new ValidationResult(
				"Timeout must be positive and no greater than Int32.MaxValue milliseconds.",
				[nameof(Timeout)]);
		}

		if (MdnsTimeout <= TimeSpan.Zero || MdnsTimeout > TimeSpan.FromSeconds(30))
		{
			yield return new ValidationResult("MdnsTimeout must be positive and no greater than 30 seconds.",
				[nameof(MdnsTimeout)]);
		}

		if (ApplicationId is not null && string.IsNullOrWhiteSpace(ApplicationId))
		{
			yield return new ValidationResult("ApplicationId cannot be empty when configured.",
				[nameof(ApplicationId)]);
		}

		if (ApplicationVersion is not null && string.IsNullOrWhiteSpace(ApplicationVersion))
		{
			yield return new ValidationResult("ApplicationVersion cannot be empty when configured.",
				[nameof(ApplicationVersion)]);
		}

		if (string.IsNullOrWhiteSpace(ApplicationName))
		{
			yield return new ValidationResult("ApplicationName is required.", [nameof(ApplicationName)]);
		}

		if (string.IsNullOrWhiteSpace(DeviceName))
		{
			yield return new ValidationResult("DeviceName is required.", [nameof(DeviceName)]);
		}

		if (string.IsNullOrWhiteSpace(CredentialProfile) || CredentialProfile.Length > 128 ||
		    CredentialProfile.Any(character => character is '/' or '\\' or '\0' || char.IsControl(character)))
		{
			yield return new ValidationResult("CredentialProfile must be a safe, non-empty identity component.",
				[nameof(CredentialProfile)]);
		}

		if (string.IsNullOrWhiteSpace(HttpClientName))
		{
			yield return new ValidationResult("HttpClientName is required.", [nameof(HttpClientName)]);
		}
	}

	public FreeboxClientOptions UseServer(Uri serverAddress)
	{
		ServerAddress = serverAddress;
		return this;
	}

	public FreeboxClientOptions UseApplication(string applicationId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
		ApplicationId = applicationId;
		return this;
	}

	public FreeboxClientOptions WithTimeout(TimeSpan timeout)
	{
		Timeout = timeout;
		return this;
	}

	public FreeboxClientOptions WithApplicationVersion(string applicationVersion)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(applicationVersion);
		ApplicationVersion = applicationVersion;
		return this;
	}

	internal FreeboxClientOptions CreateValidatedSnapshot()
	{
		var validation = new FreeboxClientOptionsValidator().Validate(name: null, this);
		if (validation.Failed)
		{
			throw new OptionsValidationException(validation.FailureMessage, typeof(FreeboxClientOptions),
				validation.Failures);
		}

		return new FreeboxClientOptions
		{
			ServerAddress = ServerAddress,
			ApiVersion = ApiVersion,
			ApiBasePath = FreeboxApiPath.NormalizeBasePath(ApiBasePath),
			Timeout = Timeout,
			EnableMdnsDiscovery = EnableMdnsDiscovery,
			MdnsTimeout = MdnsTimeout,
			ApplicationId = ApplicationId,
			ApplicationName = ApplicationName,
			ApplicationVersion = ApplicationVersion,
			DeviceName = DeviceName,
			CredentialProfile = CredentialProfile,
			AuthenticateAutomatically = AuthenticateAutomatically,
			HttpClientName = HttpClientName,
			EnableResilience = EnableResilience
		};
	}
}
