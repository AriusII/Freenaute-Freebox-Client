namespace Freenaute.Freebox.Client;

internal sealed partial class FreeboxHttpTransport(
	HttpClient httpClient,
	FreeboxClientState state,
	TimeSpan? standaloneTimeout = null) : IFreeboxTransport, IFreeboxBinaryTransport, IFreeboxWebSocketTransport
{
	internal const string AuthenticationHeader = "X-Fbx-App-Auth";
	private static readonly IFreeboxMapper Mapper = FreeboxMapper.Default;
	private static readonly FreeboxJsonSerializerContext Json = FreeboxJsonSerializerContext.Default;

	public Task<TResponse> SendContentAsync<TResponse>(HttpMethod method, string relativePath,
		HttpContent content, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		ArgumentNullException.ThrowIfNull(responseType);
		return SendTypedAsync(method, relativePath, content, responseType, requiresAuthentication, cancellationToken);
	}

	public Task SendContentAsync(HttpMethod method, string relativePath, HttpContent content,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		return SendWithoutResultAsync(method, relativePath, content, requiresAuthentication, cancellationToken);
	}

	public Task<TResponse> SendAsync<TResponse>(HttpMethod method, string relativePath,
		JsonTypeInfo<TResponse> responseType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(responseType);
		return SendTypedAsync(method, relativePath, null, responseType, requiresAuthentication, cancellationToken);
	}

	public Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string relativePath,
		TRequest request, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		ArgumentNullException.ThrowIfNull(responseType);
		return SendTypedAsync(method, relativePath, CreateJsonContent(request, requestType), responseType,
			requiresAuthentication, cancellationToken);
	}

	public Task SendAsync(HttpMethod method, string relativePath, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true) =>
		SendWithoutResultAsync(method, relativePath, null, requiresAuthentication, cancellationToken);

	public Task SendAsync<TRequest>(HttpMethod method, string relativePath, TRequest request,
		JsonTypeInfo<TRequest> requestType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		return SendWithoutResultAsync(method, relativePath, CreateJsonContent(request, requestType),
			requiresAuthentication, cancellationToken);
	}

	private static HttpContent CreateJsonContent<T>(T value, JsonTypeInfo<T> typeInfo)
	{
		var content = new ByteArrayContent(Mapper.ToUtf8Bytes(value, typeInfo));
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" };
		return content;
	}

	private async Task<TResponse> SendTypedAsync<TResponse>(HttpMethod method, string relativePath,
		HttpContent? content, JsonTypeInfo<TResponse> responseType, bool requiresAuthentication,
		CancellationToken cancellationToken)
	{
		using (content)
		using (var envelope = await SendEnvelopeAsync(method, relativePath, content, requiresAuthentication,
			       cancellationToken).ConfigureAwait(false))
		{
			if (!envelope.RootElement.TryGetProperty("result", out var result))
			{
				throw new JsonException("The Freebox response is missing its result property.");
			}

			return Mapper.FromWire(result, responseType)
			       ?? throw new JsonException("The Freebox response result cannot be null for this operation.");
		}
	}

	private async Task SendWithoutResultAsync(HttpMethod method, string relativePath, HttpContent? content,
		bool requiresAuthentication, CancellationToken cancellationToken)
	{
		using (content)
		using (await SendEnvelopeAsync(method, relativePath, content, requiresAuthentication,
			       cancellationToken).ConfigureAwait(false))
		{
		}
	}

	private async Task<JsonDocument> SendEnvelopeAsync(HttpMethod method, string relativePath, HttpContent? content,
		bool requiresAuthentication, CancellationToken cancellationToken, SessionResponse? explicitSession = null)
	{
		ArgumentNullException.ThrowIfNull(method);
		FreeboxApiPath.ValidateRelativePath(relativePath);
		cancellationToken.ThrowIfCancellationRequested();
		var apiAddress = await GetApiAddressAsync(cancellationToken).ConfigureAwait(false);
		var session = explicitSession;
		if (requiresAuthentication)
		{
			session = Volatile.Read(ref state.Session);
			if (session is null)
			{
				if (!state.Options.AuthenticateAutomatically)
				{
					throw new FreeboxAuthenticationException(
						"Open a session before making authenticated requests, or enable automatic authentication.");
				}

				session = await OpenSessionAsync(cancellationToken).ConfigureAwait(false);
			}
		}

		using var request = new HttpRequestMessage(method, FreeboxApiPath.Resolve(apiAddress, relativePath));
		request.Content = content;
		if (session is not null)
		{
			request.Headers.Add(AuthenticationHeader, session.SessionToken);
		}

		try
		{
			return await SendDocumentAsync(request, cancellationToken, true).ConfigureAwait(false);
		}
		catch (FreeboxApiException exception) when
			(session is not null && (exception.StatusCode is HttpStatusCode.Unauthorized ||
			                         exception.ErrorCode is "auth_required" or "invalid_session"))
		{
			// Invalidate this session only. In particular, never replay a mutation after an authentication failure.
			Interlocked.CompareExchange(ref state.Session, null, session);
			throw;
		}
	}

	internal async Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (Volatile.Read(ref state.ApiVersion) is { } cached)
		{
			return cached;
		}

		await state.DiscoveryGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			if (state.ApiVersion is { } existing)
			{
				return existing;
			}

			var mdns = await TryDiscoverMdnsAsync(cancellationToken).ConfigureAwait(false);
			if (mdns is not null && TryValidateMdns(mdns, out var origin, out var apiAddress))
			{
				state.ApiOrigin = origin;
				state.ApiAddress = apiAddress;
				Volatile.Write(ref state.ApiVersion, mdns.ApiVersion);
				return mdns.ApiVersion;
			}

			using var request =
				new HttpRequestMessage(HttpMethod.Get, new Uri(state.Options.ServerAddress, "api_version"));
			using var document = await SendDocumentAsync(request, cancellationToken).ConfigureAwait(false);
			var version = Mapper.FromWire(document.RootElement, Json.ApiVersionResponse)
			              ?? throw new JsonException("The Freebox discovery response cannot be null.");
			if (state.Options.ApiVersion is null)
			{
				// The advertised domain and HTTPS port can refer to remote access. The HTTP fallback must not
				// redirect credentials away from the explicitly configured origin.
				state.ApiOrigin = state.Options.ServerAddress;
				state.ApiAddress = CreateApiAddress(version.ApiBaseUrl, GetMajorVersion(version.ApiVersion),
					state.Options.ServerAddress);
			}

			Volatile.Write(ref state.ApiVersion, version);
			return version;
		}
		finally
		{
			state.DiscoveryGate.Release();
		}
	}

	private async ValueTask<FreeboxDiscoveryResult?> TryDiscoverMdnsAsync(CancellationToken cancellationToken)
	{
		if (!state.Options.EnableMdnsDiscovery)
		{
			return null;
		}

		using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		timeout.CancelAfter(state.Options.MdnsTimeout);
		try
		{
			return await state.DiscoveryResolver.DiscoverAsync(timeout.Token).ConfigureAwait(false);
		}
		catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
		{
			return null;
		}
		catch (SocketException)
		{
			return null;
		}
		catch (IOException)
		{
			return null;
		}
	}

	private static bool TryValidateMdns(FreeboxDiscoveryResult result, out Uri? origin, out Uri? apiAddress)
	{
		origin = null;
		apiAddress = null;
		var version = result.ApiVersion;
		if (version is null || result.Origin is null || !result.Origin.IsAbsoluteUri ||
		    !string.Equals(result.Origin.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal) ||
		    !string.IsNullOrEmpty(result.Origin.UserInfo) || !string.IsNullOrEmpty(result.Origin.Query) ||
		    !string.IsNullOrEmpty(result.Origin.Fragment) ||
		    !string.Equals(result.Origin.AbsolutePath, "/", StringComparison.Ordinal) ||
		    string.IsNullOrWhiteSpace(version.Uid) || version.Uid.Length > 128 || version.Uid.Any(char.IsControl) ||
		    version.HttpsAvailable is not true || version.HttpsPort is not (> 0 and <= 65535) ||
		    string.IsNullOrWhiteSpace(version.ApiDomain) || result.Origin.Port != version.HttpsPort || !string.Equals(
			    result.Origin.Host.TrimEnd('.'), version.ApiDomain.TrimEnd('.'),
			    StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		try
		{
			int major = GetMajorVersion(version.ApiVersion);
			apiAddress = new Uri(result.Origin,
				FreeboxApiPath.NormalizeBasePath(version.ApiBaseUrl).TrimStart('/') +
				$"v{major.ToString(CultureInfo.InvariantCulture)}/");
		}
		catch (Exception exception) when (exception is ArgumentException or FormatException or JsonException)
		{
			return false;
		}

		origin = result.Origin;
		return true;
	}

	private async Task<Uri> GetApiAddressAsync(CancellationToken cancellationToken)
	{
		if (state.Options.ApiVersion is { } major)
		{
			return CreateApiAddress(state.Options.ApiBasePath, major, state.Options.ServerAddress);
		}

		await GetApiVersionAsync(cancellationToken).ConfigureAwait(false);
		return state.ApiAddress!;
	}

	private static Uri CreateApiAddress(string basePath, int major, Uri origin)
	{
		return new Uri(origin,
			FreeboxApiPath.NormalizeBasePath(basePath).TrimStart('/') +
			$"v{major.ToString(CultureInfo.InvariantCulture)}/");
	}

	private static int GetMajorVersion(string version)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(version);
		int separator = version.IndexOf('.', StringComparison.OrdinalIgnoreCase);
		var majorPart = separator < 0 ? version.AsSpan() : version.AsSpan(0, separator);
		if (!int.TryParse(majorPart, NumberStyles.None, CultureInfo.InvariantCulture, out int major) || major <= 0)
		{
			throw new JsonException("The Freebox advertised an invalid API major version.");
		}

		return major;
	}

	internal async Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (Volatile.Read(ref state.Session) is { } session)
		{
			return session;
		}

		var options = state.Options;
		if (string.IsNullOrWhiteSpace(options.ApplicationId))
		{
			throw new FreeboxAuthenticationException("Configure ApplicationId before opening a Freebox session.");
		}

		// Resolve a stable UID when the credential came from DI. Standalone callers provide credentials directly and
		// may use an explicit API address without making an additional discovery request.
		if (state.StandaloneCredentials is null)
		{
			await GetApiVersionAsync(cancellationToken).ConfigureAwait(false);
		}

		var credentialId = state.GetCredentialId();

		await state.SessionGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			if (state.Session is { } existing)
			{
				return existing;
			}

			var credentials = await GetApprovedCredentialsAsync(credentialId, cancellationToken).ConfigureAwait(false);
			var login = await SendAsync(HttpMethod.Get, "login/", Json.LoginResponse, cancellationToken,
				false).ConfigureAwait(false);
			ArgumentException.ThrowIfNullOrWhiteSpace(login.Challenge);
			string password = CreateSessionPassword(credentials.GetAppToken(), login.Challenge);
			var opened = await SendAsync(HttpMethod.Post, "login/session/",
				new SessionStartRequest(options.ApplicationId, password, options.ApplicationVersion),
				Json.SessionStartRequest,
				Json.SessionResponse, cancellationToken, false).ConfigureAwait(false);
			if (string.IsNullOrWhiteSpace(opened.SessionToken) || opened.SessionToken.Any(char.IsControl))
			{
				throw new JsonException("The Freebox returned an invalid session token.");
			}

			Volatile.Write(ref state.Session, opened);
			return opened;
		}
		finally
		{
			state.SessionGate.Release();
		}
	}

	private async ValueTask<FreeboxAppCredentials> GetApprovedCredentialsAsync(FreeboxCredentialId credentialId,
		CancellationToken cancellationToken)
	{
		if (!state.CredentialStore.IsAvailable)
		{
			throw new FreeboxAuthenticationException(
				$"The '{state.CredentialStore.Kind}' credential store is unavailable.");
		}

		var credentials = await state.CredentialStore.GetAsync(credentialId, cancellationToken).ConfigureAwait(false);
		if (credentials is null && state.StandaloneCredentials is not null)
		{
			credentials = state.StandaloneCredentials;
			await state.CredentialStore.PutAsync(credentialId, credentials, cancellationToken).ConfigureAwait(false);
		}

		if (credentials is null || credentials.Status is not FreeboxCredentialStatus.Approved)
		{
			throw new FreeboxAuthenticationException(credentials?.Status is FreeboxCredentialStatus.Pending
				? "Complete the physical Freebox approval before opening a session."
				: "No approved reusable application credentials are available. Begin Freebox pairing first.");
		}

		if (!StringComparer.Ordinal.Equals(credentials.ApplicationId, state.Options.ApplicationId))
		{
			throw new FreeboxAuthenticationException("Stored credentials belong to a different application identity.");
		}

		return credentials;
	}

	private static string CreateSessionPassword(string appToken, string challenge)
	{
		byte[] key = Encoding.UTF8.GetBytes(appToken);
		byte[] challengeBytes = Encoding.UTF8.GetBytes(challenge);
		try
		{
			// HMAC-SHA1 is mandated by the Freebox authentication wire protocol.
			byte[] digest = HMACSHA1.HashData(key, challengeBytes);
			try
			{
				return Convert.ToHexStringLower(digest);
			}
			finally
			{
				CryptographicOperations.ZeroMemory(digest);
			}
		}
		finally
		{
			CryptographicOperations.ZeroMemory(key);
			CryptographicOperations.ZeroMemory(challengeBytes);
		}
	}

	internal async Task CloseSessionAsync(CancellationToken cancellationToken)
	{
		await state.SessionGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			var session = Interlocked.Exchange(ref state.Session, null);
			if (session is null)
			{
				return;
			}

			using var result = await SendEnvelopeAsync(HttpMethod.Post, "login/logout/", null,
				false, cancellationToken, session).ConfigureAwait(false);
		}
		finally
		{
			state.SessionGate.Release();
		}
	}

	private async Task<JsonDocument> SendDocumentAsync(HttpRequestMessage request, CancellationToken cancellationToken,
		bool validateEnvelope = false)
	{
		// HttpClient's timeout ends at the headers when ResponseHeadersRead is used. Keep the configured
		// timeout active while reading and parsing the response body as well.
		// Standalone clients preserve the caller's HttpClient configuration and enforce their own operation
		// timeout. DI clients use the configured HttpClient value to honor ConfigureHttpClient overrides.
		var timeout = standaloneTimeout ?? httpClient.Timeout;
		using var operationTimeout = timeout == Timeout.InfiniteTimeSpan
			? null
			: CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		operationTimeout?.CancelAfter(timeout);
		var operationCancellation = operationTimeout?.Token ?? cancellationToken;
		using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
			operationCancellation).ConfigureAwait(false);
		var stream = await response.Content.ReadAsStreamAsync(operationCancellation).ConfigureAwait(false);
		await using var streamLifetime = stream.ConfigureAwait(false);
		JsonDocument document;
		try
		{
			document = await JsonDocument.ParseAsync(stream, cancellationToken: operationCancellation)
				.ConfigureAwait(false);
		}
		catch (JsonException) when (!response.IsSuccessStatusCode)
		{
			throw new FreeboxApiException(response.StatusCode);
		}

		if (!response.IsSuccessStatusCode)
		{
			using (document)
			{
				throw CreateApiException(response.StatusCode, document.RootElement);
			}
		}

		if (validateEnvelope)
		{
			try
			{
				EnsureEnvelopeSuccess(document.RootElement, response.StatusCode);
			}
			catch
			{
				document.Dispose();
				throw;
			}
		}

		return document;
	}

	private static void EnsureEnvelopeSuccess(JsonElement root, HttpStatusCode statusCode)
	{
		if (root.ValueKind is not JsonValueKind.Object || !root.TryGetProperty("success", out var success) ||
		    success.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
		{
			throw new JsonException("The Freebox response must contain a boolean success property.");
		}

		if (!success.GetBoolean())
		{
			throw CreateApiException(statusCode, root);
		}
	}

	private static FreeboxApiException CreateApiException(HttpStatusCode statusCode, JsonElement root)
	{
		if (root.ValueKind is not JsonValueKind.Object)
		{
			return new FreeboxApiException(statusCode);
		}

		JsonElement? details = root.TryGetProperty("result", out var result) ? result.Clone() : null;
		return new FreeboxApiException(statusCode, ReadString(root, "error_code"), ReadString(root, "msg"),
			ReadString(root, "uid"), details);
	}

	private static string? ReadString(JsonElement root, string property)
	{
		return root.TryGetProperty(property, out var value) && value.ValueKind is JsonValueKind.String
			? value.GetString()
			: null;
	}
}
