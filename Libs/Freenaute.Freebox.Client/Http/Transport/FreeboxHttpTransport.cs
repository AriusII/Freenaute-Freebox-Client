namespace Freenaute.Freebox.Client.Http.Transport;

/// <summary>
///     JSON envelope transport over the shared <see cref="FreeboxHttpTransportCore" />. Binary downloads and
///     WebSockets live in <see cref="Binary.FreeboxBinaryTransport" /> and
///     <see cref="WebSockets.FreeboxWebSocketTransport" />.
/// </summary>
internal sealed class FreeboxHttpTransport(FreeboxHttpTransportCore core) : IFreeboxTransport
{
	public async Task<TResponse> SendContentAsync<TResponse>(HttpMethod method, string relativePath,
		HttpContent content, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		ArgumentNullException.ThrowIfNull(responseType);
		return await core.SendTypedAsync(method, relativePath, content, responseType, requiresAuthentication,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task SendContentAsync(HttpMethod method, string relativePath, HttpContent content,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		await core.SendWithoutResultAsync(method, relativePath, content, requiresAuthentication, cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string relativePath,
		JsonTypeInfo<TResponse> responseType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(responseType);
		return await core
			.SendTypedAsync(method, relativePath, content: null, responseType, requiresAuthentication, cancellationToken)
			.ConfigureAwait(false);
	}

	public async Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string relativePath,
		TRequest request, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		ArgumentNullException.ThrowIfNull(responseType);
		return await core.SendTypedAsync(method, relativePath,
			FreeboxHttpTransportCore.CreateJsonContent(request, requestType), responseType,
			requiresAuthentication, cancellationToken).ConfigureAwait(false);
	}

	public async Task SendAsync(HttpMethod method, string relativePath, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true) =>
		await core.SendWithoutResultAsync(method, relativePath, content: null, requiresAuthentication, cancellationToken)
			.ConfigureAwait(false);

	public async Task SendAsync<TRequest>(HttpMethod method, string relativePath, TRequest request,
		JsonTypeInfo<TRequest> requestType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		await core.SendWithoutResultAsync(method, relativePath,
			FreeboxHttpTransportCore.CreateJsonContent(request, requestType),
			requiresAuthentication, cancellationToken).ConfigureAwait(false);
	}

	internal async Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken) =>
		await core.GetApiVersionAsync(cancellationToken).ConfigureAwait(false);

	internal async Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken) =>
		await core.OpenSessionAsync(cancellationToken).ConfigureAwait(false);

	internal async Task CloseSessionAsync(CancellationToken cancellationToken) =>
		await core.CloseSessionAsync(cancellationToken).ConfigureAwait(false);
}
