using Freenaute.Freebox.Client.Domains.Protocol.Support;

namespace Freenaute.Freebox.Client.Domains.Protocol.Cameras;

internal sealed class FreeboxCamerasApi(IFreeboxTransport transport) : IFreeboxCamerasApi
{
	public async Task<Camera[]> ListAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ProtocolConstants.Routes.Cameras, ProtocolJsonSerializerContext.Default.CameraArray,
			cancellationToken).ConfigureAwait(false);
	}

	public FreeboxCamera Camera(string id) => new(transport, id);
}
