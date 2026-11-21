namespace Freenaute.Freebox.Client.Http.Transport;

/// <summary>
///     JSON envelope transport over the shared <see cref="FreeboxHttpTransportCore" />. Binary downloads and
///     WebSockets live in <see cref="Binary.FreeboxBinaryTransport" /> and
///     <see cref="WebSockets.FreeboxWebSocketTransport" />.
/// </summary>
internal sealed class FreeboxHttpTransport(FreeboxHttpTransportCore core) : IFreeboxTransport
{
	public Task<TResponse> SendContentAsync<TResponse>(HttpMethod method, string relativePath,
		HttpContent content, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		ArgumentNullException.ThrowIfNull(responseType);
		return core.SendTypedAsync(method, relativePath, content, responseType, requiresAuthentication,
			cancellationToken);
	}

	public Task SendContentAsync(HttpMethod method, string relativePath, HttpContent content,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(content);
		return core.SendWithoutResultAsync(method, relativePath, content, requiresAuthentication, cancellationToken);
	}

	public Task<TResponse> SendAsync<TResponse>(HttpMethod method, string relativePath,
		JsonTypeInfo<TResponse> responseType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(responseType);
		return core.SendTypedAsync(method, relativePath, null, responseType, requiresAuthentication, cancellationToken);
	}

	public Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string relativePath,
		TRequest request, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		ArgumentNullException.ThrowIfNull(responseType);
		return core.SendTypedAsync(method, relativePath,
			FreeboxHttpTransportCore.CreateJsonContent(request, requestType), responseType,
			requiresAuthentication, cancellationToken);
	}

	public Task SendAsync(HttpMethod method, string relativePath, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true) =>
		core.SendWithoutResultAsync(method, relativePath, null, requiresAuthentication, cancellationToken);

	public Task SendAsync<TRequest>(HttpMethod method, string relativePath, TRequest request,
		JsonTypeInfo<TRequest> requestType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(requestType);
		return core.SendWithoutResultAsync(method, relativePath,
			FreeboxHttpTransportCore.CreateJsonContent(request, requestType),
			requiresAuthentication, cancellationToken);
	}

	internal Task<ApiVersionResponse> GetApiVersionAsync(CancellationToken cancellationToken) =>
		core.GetApiVersionAsync(cancellationToken);

	internal Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken) =>
		core.OpenSessionAsync(cancellationToken);

	internal Task CloseSessionAsync(CancellationToken cancellationToken) =>
		core.CloseSessionAsync(cancellationToken);
}
