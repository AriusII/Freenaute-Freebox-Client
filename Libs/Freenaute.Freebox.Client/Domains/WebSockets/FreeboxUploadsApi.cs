namespace Freenaute.Freebox.Client.Domains.WebSockets;

public sealed class FreeboxUploadsApi
{
    private readonly IFreeboxWebSocketTransport _transport;

    public FreeboxUploadsApi(IFreeboxWebSocketTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    public FreeboxUploadDirectory To(EncodedFreeboxPath directory)
    {
        return new FreeboxUploadDirectory(_transport, directory);
    }
}
