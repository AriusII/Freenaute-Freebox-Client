namespace Freenaute.Freebox.Client.Domains.WebSockets.Uploads;

public sealed class FreeboxWebSocketUploadsApi
{
	private readonly IFreeboxWebSocketTransport _transport;

	public FreeboxWebSocketUploadsApi(IFreeboxWebSocketTransport transport)
	{
		ArgumentNullException.ThrowIfNull(transport);
		_transport = transport;
	}

	public FreeboxUploadDirectory To(EncodedFreeboxPath directory) => new(_transport, directory);
}
