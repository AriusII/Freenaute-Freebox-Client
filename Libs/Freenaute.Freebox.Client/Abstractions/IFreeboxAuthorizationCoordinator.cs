namespace Freenaute.Freebox.Client.Abstractions;

/// <summary>Coordinates the one-time physical Freebox approval and reusable app-token lifecycle.</summary>
public interface IFreeboxAuthorizationCoordinator
{
	ValueTask<FreeboxApprovalRegistration> BeginPairingAsync(CancellationToken cancellationToken = default);

	ValueTask<FreeboxAppCredentials> WaitForApprovalAsync(TimeSpan? pollInterval = null,
		CancellationToken cancellationToken = default);

	ValueTask ForgetAsync(CancellationToken cancellationToken = default);
}
