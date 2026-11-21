namespace Freenaute.Freebox.Client;

public sealed class FreeboxAuthorizationException : InvalidOperationException
{
	public FreeboxAuthorizationException(AuthorizationTrackEnum status)
		: base($"Freebox authorization ended with status '{status}'.")
	{
		Status = status;
	}

	public AuthorizationTrackEnum Status { get; }
}
