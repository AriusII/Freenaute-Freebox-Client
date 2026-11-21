namespace Freenaute.Freebox.Client;

public static class FreeboxServiceCollectionExtensions
{
	/// <summary>Default HTTP client name used when none is configured.</summary>
	public const string DefaultHttpClientName = "Freenaute.Freebox.Client";

	public static IHttpClientBuilder AddFreeboxClient(this IServiceCollection services,
		Action<FreeboxClientOptions> configure)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configure);
		return AddCore(services, null, configure, null, false);
	}

	public static IHttpClientBuilder AddFreeboxClient(this IServiceCollection services, IConfiguration section)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(section);
		return AddCore(services, null, null, section, false);
	}

	public static IHttpClientBuilder AddFreeboxClient(this IServiceCollection services,
		Action<FreeboxClientOptions> configure, bool validateOnStart)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configure);
		return AddCore(services, null, configure, null, validateOnStart);
	}

	public static IHttpClientBuilder AddFreeboxClient(this IServiceCollection services, string name,
		Action<FreeboxClientOptions> configure)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentNullException.ThrowIfNull(configure);
		return AddCore(services, name, configure, null, false);
	}

	private static IHttpClientBuilder AddCore(IServiceCollection services, string? keyedName,
		Action<FreeboxClientOptions>? configure, IConfiguration? section, bool validateOnStart)
	{
		if (services.Any(descriptor => descriptor.ServiceType == typeof(FreeboxClientState) &&
		                               Equals(descriptor.ServiceKey, keyedName)))
		{
			string owner = keyedName is null ? "the default client" : $"client '{keyedName}'";
			throw new InvalidOperationException($"AddFreeboxClient cannot register {owner} more than once.");
		}

		var registrationOptions = new FreeboxClientOptions();
		configure?.Invoke(registrationOptions);
		if (section is not null)
		{
			registrationOptions.HttpClientName = section[nameof(FreeboxClientOptions.HttpClientName)]
			                                     ?? registrationOptions.HttpClientName;
			registrationOptions.EnableResilience = ReadBoolean(section,
				nameof(FreeboxClientOptions.EnableResilience), registrationOptions.EnableResilience);
		}

		string httpClientName = ResolveHttpClientName(registrationOptions.HttpClientName, keyedName);

		services.TryAddSingleton<IFreeboxCredentialStore, InMemoryFreeboxCredentialStore>();
		services.TryAddSingleton<IFreeboxDiscoveryResolver, FreeboxMdnsDiscoveryResolver>();
		services.AddSingleton<IValidateOptions<FreeboxClientOptions>, FreeboxClientOptionsValidator>();

		var optionsBuilder = keyedName is null
			? services.AddOptions<FreeboxClientOptions>()
			: services.AddOptions<FreeboxClientOptions>(keyedName);
		if (configure is not null)
		{
			optionsBuilder.Configure(configure);
		}

		if (section is not null)
		{
			optionsBuilder.Bind(section);
		}

		if (validateOnStart)
		{
			optionsBuilder.ValidateOnStart();
		}

		if (keyedName is null)
		{
			services.AddSingleton(sp => CreateState(sp, null));
		}
		else
		{
			services.AddKeyedSingleton<FreeboxClientState>(keyedName, (sp, _) => CreateState(sp, keyedName));
		}

		IHttpClientBuilder builder;
		if (keyedName is null)
		{
			builder = services.AddHttpClient(httpClientName, (sp, client) =>
				{
					var state = sp.GetRequiredService<FreeboxClientState>();
					client.BaseAddress = state.Options.ServerAddress;
					client.Timeout = state.Options.Timeout;
				})
				.ConfigurePrimaryHttpMessageHandler(() => FreeboxTls.CreateHandler())
				.RedactLoggedHeaders(["X-Fbx-App-Auth", "Authorization", "Cookie", "Set-Cookie"]);
			services.AddSingleton<IFreeboxClient>(sp =>
				new FreeboxClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName),
					sp.GetRequiredService<FreeboxClientState>()));
		}
		else
		{
			services.AddKeyedSingleton<IFreeboxClient>(keyedName, (sp, _) =>
				new FreeboxClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient(httpClientName),
					sp.GetRequiredKeyedService<FreeboxClientState>(keyedName)));
			builder = services.AddHttpClient(httpClientName, (sp, client) =>
				{
					var state = sp.GetRequiredKeyedService<FreeboxClientState>(keyedName);
					client.BaseAddress = state.Options.ServerAddress;
					client.Timeout = state.Options.Timeout;
				})
				.ConfigurePrimaryHttpMessageHandler(() => FreeboxTls.CreateHandler())
				.RedactLoggedHeaders(["X-Fbx-App-Auth", "Authorization", "Cookie", "Set-Cookie"]);
		}

		if (registrationOptions.EnableResilience)
		{
			builder.AddStandardResilienceHandler(options => options.Retry.DisableForUnsafeHttpMethods());
		}

		RegisterOwnerServices(services, keyedName);
		return builder;
	}

	private static FreeboxClientState CreateState(IServiceProvider serviceProvider, string? keyedName)
	{
		var options = keyedName is null
			? serviceProvider.GetRequiredService<IOptions<FreeboxClientOptions>>().Value
			: serviceProvider.GetRequiredService<IOptionsMonitor<FreeboxClientOptions>>().Get(keyedName);
		var store = keyedName is null
			? serviceProvider.GetRequiredService<IFreeboxCredentialStore>()
			: serviceProvider.GetKeyedService<IFreeboxCredentialStore>(keyedName) ??
			  serviceProvider.GetRequiredService<IFreeboxCredentialStore>();
		var resolver = keyedName is null
			? serviceProvider.GetRequiredService<IFreeboxDiscoveryResolver>()
			: serviceProvider.GetKeyedService<IFreeboxDiscoveryResolver>(keyedName) ??
			  serviceProvider.GetRequiredService<IFreeboxDiscoveryResolver>();
		return new FreeboxClientState(options.CreateValidatedSnapshot(), store, resolver);
	}

	private static string ResolveHttpClientName(string configuredName, string? keyedName)
	{
		if (string.IsNullOrWhiteSpace(configuredName))
		{
			configuredName = DefaultHttpClientName;
		}

		if (keyedName is null || !StringComparer.Ordinal.Equals(configuredName, DefaultHttpClientName))
		{
			return configuredName;
		}

		byte[] keyBytes = Encoding.UTF8.GetBytes(keyedName);
		try
		{
			return $"{DefaultHttpClientName}.{Convert.ToHexStringLower(keyBytes)}";
		}
		finally
		{
			CryptographicOperations.ZeroMemory(keyBytes);
		}
	}

	private static bool ReadBoolean(IConfiguration section, string key, bool defaultValue)
	{
		string? value = section[key];
		if (value is null)
		{
			return defaultValue;
		}

		if (bool.TryParse(value, out bool result))
		{
			return result;
		}

		throw new OptionsValidationException(nameof(FreeboxClientOptions), typeof(FreeboxClientOptions),
			[$"{key} must be either 'true' or 'false'."]);
	}

	private static void RegisterOwnerServices(IServiceCollection services, string? keyedName)
	{
		if (keyedName is null)
		{
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Discovery);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Authentication);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().AirMedia);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Cameras);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Notifications);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Network);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Services);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Files);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().SystemHome);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().WebSockets);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().Transport);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().BinaryTransport);
			services.AddTransient(static sp => sp.GetRequiredService<IFreeboxClient>().WebSocketTransport);
			services.AddSingleton<IFreeboxAuthorizationCoordinator>(sp =>
			{
				var state = sp.GetRequiredService<FreeboxClientState>();
				return new FreeboxAuthorizationCoordinator(sp.GetRequiredService<IFreeboxClient>(), state.Options,
					state.CredentialStore);
			});
			return;
		}

		services.AddKeyedTransient<IFreeboxDiscoveryApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Discovery);
		services.AddKeyedTransient<IFreeboxAuthenticationApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Authentication);
		services.AddKeyedTransient<IFreeboxAirMediaApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).AirMedia);
		services.AddKeyedTransient<IFreeboxCamerasApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Cameras);
		services.AddKeyedTransient<IFreeboxNotificationsApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Notifications);
		services.AddKeyedTransient<IFreeboxNetworkApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Network);
		services.AddKeyedTransient<IFreeboxServicesApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Services);
		services.AddKeyedTransient<IFreeboxFilesApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Files);
		services.AddKeyedTransient<IFreeboxSystemHomeApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).SystemHome);
		services.AddKeyedTransient<IFreeboxWebSocketsApi>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).WebSockets);
		services.AddKeyedTransient<IFreeboxTransport>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).Transport);
		services.AddKeyedTransient<IFreeboxBinaryTransport>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).BinaryTransport);
		services.AddKeyedTransient<IFreeboxWebSocketTransport>(keyedName,
			(sp, key) => sp.GetRequiredKeyedService<IFreeboxClient>(key).WebSocketTransport);
		services.AddKeyedSingleton<IFreeboxAuthorizationCoordinator>(keyedName, (sp, key) =>
		{
			var state = sp.GetRequiredKeyedService<FreeboxClientState>(key);
			return new FreeboxAuthorizationCoordinator(sp.GetRequiredKeyedService<IFreeboxClient>(key), state.Options,
				state.CredentialStore);
		});
	}
}
