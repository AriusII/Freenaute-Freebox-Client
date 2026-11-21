namespace Freenaute.Freebox.Client.Domains.WebSockets;

public enum FreeboxUploadState
{
	Starting,
	Ready,
	Sending,
	Completed,
	Cancelled,
	Faulted,
	Disposed
}
