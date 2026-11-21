namespace Freenaute.Freebox.Client.Domains.Protocol;

public sealed class FreeboxCamerasApi(IFreeboxTransport transport) : IFreeboxCamerasApi
{
    public Task<Camera[]> ListAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "camera/", ProtocolJsonSerializerContext.Default.CameraArray,
            cancellationToken);
    }

    public FreeboxCamera Camera(string id)
    {
        return new FreeboxCamera(transport, id);
    }
}
