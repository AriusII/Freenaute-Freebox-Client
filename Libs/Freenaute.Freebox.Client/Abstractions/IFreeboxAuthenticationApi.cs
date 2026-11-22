namespace Freenaute.Freebox.Client.Abstractions;

public interface IFreeboxAuthenticationApi
{
	Task<AuthorizeResponse> AuthorizeAsync(TokenRequest request, CancellationToken cancellationToken = default);
	Task<AuthorizeTrackResponse> TrackAuthorizationAsync(int trackId, CancellationToken cancellationToken = default);

	/// <summary>Polls asynchronously until authorization leaves the pending state; the caller controls cancellation.</summary>
	Task<AuthorizeTrackResponse> WaitForAuthorizationAsync(int trackId, TimeSpan? pollInterval = null,
		CancellationToken cancellationToken = default);

	Task<LoginResponse> GetLoginAsync(CancellationToken cancellationToken = default);

	/// <summary>Lists the active Freebox login sessions.</summary>
	Task<JsonElement[]> GetSessionsAsync(CancellationToken cancellationToken = default);

	Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken = default);

	/// <summary>Closes the current session.</summary>
	Task LogoutAsync(CancellationToken cancellationToken = default);

	/// <summary>Closes the current session.</summary>
	Task CloseSessionAsync(CancellationToken cancellationToken = default);
}
