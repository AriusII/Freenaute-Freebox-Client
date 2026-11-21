namespace Freenaute.Freebox.Client.Domains.Protocol.Cameras;

internal sealed class FreeboxCamerasApi(IFreeboxTransport transport) : IFreeboxCamerasApi
{
	public Task<Camera[]> ListAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "camera/", ProtocolJsonSerializerContext.Default.CameraArray,
			cancellationToken);
	}

	public FreeboxCamera Camera(string id) => new(transport, id);
}
