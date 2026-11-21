namespace Freenaute.Freebox.Client;

public sealed class FreeboxAuthorizationException(AuthorizationTrackEnum status)
	: InvalidOperationException($"Freebox authorization ended with status '{status}'.")
{
	public AuthorizationTrackEnum Status { get; } = status;
}
