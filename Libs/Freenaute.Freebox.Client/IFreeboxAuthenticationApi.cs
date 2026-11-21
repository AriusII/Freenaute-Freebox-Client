namespace Freenaute.Freebox.Client;

public interface IFreeboxAuthenticationApi
{
    Task<AuthorizeResponse> AuthorizeAsync(TokenRequest request, CancellationToken cancellationToken = default);
    Task<AuthorizeTrackResponse> TrackAuthorizationAsync(int trackId, CancellationToken cancellationToken = default);

    /// <summary>Polls asynchronously until authorization leaves the pending state; the caller controls cancellation.</summary>
    Task<AuthorizeTrackResponse> WaitForAuthorizationAsync(int trackId, TimeSpan? pollInterval = null,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> GetLoginAsync(CancellationToken cancellationToken = default);
    Task<SessionResponse> OpenSessionAsync(CancellationToken cancellationToken = default);
    Task CloseSessionAsync(CancellationToken cancellationToken = default);
}
