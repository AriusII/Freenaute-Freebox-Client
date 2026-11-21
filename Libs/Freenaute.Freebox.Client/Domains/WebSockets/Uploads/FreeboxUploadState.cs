namespace Freenaute.Freebox.Client.Domains.WebSockets.Uploads;

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
