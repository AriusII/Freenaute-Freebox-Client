namespace Freenaute.Freebox.Client.Domains.WebSockets;

public interface IFreeboxWebSocketsApi
{
	FreeboxEventsApi Events { get; }
	FreeboxUploadsApi Uploads { get; }
}
