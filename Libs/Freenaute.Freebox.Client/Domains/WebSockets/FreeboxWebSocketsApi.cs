namespace Freenaute.Freebox.Client.Domains.WebSockets;

/// <summary>Event subscriptions and modern binary file uploads over authenticated WebSockets.</summary>
internal sealed class FreeboxWebSocketsApi : IFreeboxWebSocketsApi
{
	public FreeboxWebSocketsApi(IFreeboxWebSocketTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		Events = new FreeboxEventsApi(transport);
		Uploads = new FreeboxWebSocketUploadsApi(transport);
	}

	public FreeboxEventsApi Events { get; }
	public FreeboxWebSocketUploadsApi Uploads { get; }
}
