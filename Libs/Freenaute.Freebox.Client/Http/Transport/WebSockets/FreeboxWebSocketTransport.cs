namespace Freenaute.Freebox.Client.Http.Transport.WebSockets;

/// <summary>Opens authenticated WebSockets over the shared HTTP transport core.</summary>
internal sealed class FreeboxWebSocketTransport(FreeboxHttpTransportCore core) : IFreeboxWebSocketTransport
{
	public async Task<FreeboxWebSocketConnection> ConnectAsync(string relativePath,
		CancellationToken cancellationToken = default)
	{
		FreeboxApiPath.ValidateRelativePath(relativePath);
		cancellationToken.ThrowIfCancellationRequested();
		var socket = new ClientWebSocket();
		socket.Options.CollectHttpResponseDetails = true;
		SessionResponse? session = null;
		var timeout = core.EffectiveTimeout;
		using var handshake = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		if (timeout != Timeout.InfiniteTimeSpan)
		{
			handshake.CancelAfter(timeout);
		}

		try
		{
			var apiAddress = await core.GetApiAddressAsync(handshake.Token).ConfigureAwait(false);
			session = await core.ResolveSessionAsync(requiresAuthentication: true,
				"Open a session before opening an authenticated WebSocket.", handshake.Token).ConfigureAwait(false);
			var address = FreeboxApiPath.Resolve(apiAddress, relativePath);
			var websocketAddress = new UriBuilder(address)
			{
				Scheme = string.Equals(address.Scheme, Uri.UriSchemeHttps, StringComparison.Ordinal) ? "wss" : "ws"
			}.Uri;
			socket.Options.SetRequestHeader(FreeboxHttpTransportCore.AuthenticationHeader, session!.SessionToken);
			// The provided invoker retains the configured TLS roots, hostname validation and DI handlers.
			await socket.ConnectAsync(websocketAddress, core.HttpClient, handshake.Token).ConfigureAwait(false);
			return new FreeboxWebSocketConnection(socket);
		}
		catch (WebSocketException error) when (session is not null &&
		                                       (socket.HttpStatusCode is HttpStatusCode.Unauthorized
			                                        or HttpStatusCode.Forbidden ||
		                                        error.InnerException is HttpRequestException
		                                        {
			                                        StatusCode: HttpStatusCode.Unauthorized
			                                        or HttpStatusCode.Forbidden
		                                        }))
		{
			core.InvalidateSession(session);
			socket.Dispose();
			throw;
		}
		catch
		{
			socket.Dispose();
			throw;
		}
	}
}
