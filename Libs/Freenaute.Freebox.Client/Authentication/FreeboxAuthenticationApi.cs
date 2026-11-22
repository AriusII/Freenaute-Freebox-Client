namespace Freenaute.Freebox.Client.Authentication;

internal sealed class FreeboxAuthenticationApi(FreeboxHttpTransport transport) : IFreeboxAuthenticationApi
{
	private static readonly FreeboxJsonSerializerContext Json = FreeboxJsonSerializerContext.Default;

	public async Task<AuthorizeResponse> AuthorizeAsync(TokenRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.AppId);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.AppName);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.AppVersion);
		ArgumentException.ThrowIfNullOrWhiteSpace(request.DeviceName);
		return await transport.SendAsync(HttpMethod.Post, ClientConstants.Routes.LoginAuthorize, request,
			Json.TokenRequest, Json.AuthorizeResponse, cancellationToken, requiresAuthentication: false).ConfigureAwait(false);
	}

	public async Task<AuthorizeTrackResponse> TrackAuthorizationAsync(int trackId,
		CancellationToken cancellationToken = default)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(trackId);
		return await transport.SendAsync(HttpMethod.Get,
			$"{ClientConstants.Routes.LoginAuthorize}{trackId.ToString(CultureInfo.InvariantCulture)}/",
			Json.AuthorizeTrackResponse, cancellationToken, requiresAuthentication: false).ConfigureAwait(false);
	}

	public async Task<AuthorizeTrackResponse> WaitForAuthorizationAsync(int trackId, TimeSpan? pollInterval = null,
		CancellationToken cancellationToken = default)
	{
		var interval = pollInterval ?? TimeSpan.FromSeconds(1);
		if (interval <= TimeSpan.Zero || interval.TotalMilliseconds > uint.MaxValue - 1)
		{
			throw new ArgumentOutOfRangeException(nameof(pollInterval));
		}

		while (true)
		{
			var result = await TrackAuthorizationAsync(trackId, cancellationToken).ConfigureAwait(false);
			if (result.Status is not AuthorizationTrackEnum.Pending)
			{
				return result;
			}

			await Task.Delay(interval, cancellationToken).ConfigureAwait(false);
		}
	}

	public async Task<LoginResponse> GetLoginAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ClientConstants.Routes.Login, Json.LoginResponse,
			cancellationToken,
requiresAuthentication: false).ConfigureAwait(false);
	}

	public async Task<JsonElement[]> GetSessionsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ClientConstants.Routes.LoginSession, Json.JsonElementArray,
			cancellationToken, requiresAuthentication: false).ConfigureAwait(false);
	}

	public async Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken = default) =>
		await transport.OpenSessionAsync(cancellationToken).ConfigureAwait(false);

	public async Task LogoutAsync(CancellationToken cancellationToken = default) =>
		await transport.CloseSessionAsync(cancellationToken).ConfigureAwait(false);

	public async Task CloseSessionAsync(CancellationToken cancellationToken = default) =>
		await transport.CloseSessionAsync(cancellationToken).ConfigureAwait(false);
}
