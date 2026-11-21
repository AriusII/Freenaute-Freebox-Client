namespace Freenaute.Freebox.Client.Domains.Protocol;

public sealed class FreeboxCamera
{
    private readonly string _path;
    private readonly IFreeboxTransport _transport;

    internal FreeboxCamera(IFreeboxTransport transport, string id)
    {
        _transport = transport;
        _path = $"camera/{FreeboxApiPath.EncodeSegment(id)}";
    }

    public Task<Camera> GetAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Get, _path, ProtocolJsonSerializerContext.Default.Camera,
            cancellationToken);
    }
}
