namespace Freenaute.Freebox.Client.Domains.WebSockets.Uploads;

public enum FreeboxUploadState : byte
{
	Starting,
	Ready,
	Sending,
	Completed,
	Cancelled,
	Faulted,
	Disposed
}
