namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxNetworkControlResource
{
	private readonly string path;
	private readonly IFreeboxTransport transport;

	internal FreeboxNetworkControlResource(IFreeboxTransport transport, long id)
	{
		this.transport = transport;
		path = "network_control/" + SystemHomeWire.Id(id);
		Rules = new FreeboxNetworkControlRulesApi(transport, id);
	}

	public FreeboxNetworkControlRulesApi Rules { get; }

	public Task<NetworkControl> GetAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.NetworkControl, cancellationToken);

	public FreeboxSystemHomeCommand<NetworkControlUpdate, NetworkControl> Configure()
	{
		return new FreeboxSystemHomeCommand<NetworkControlUpdate, NetworkControl>(transport, HttpMethod.Put, path,
			new NetworkControlUpdate(), SystemHomeWire.Json.NetworkControlUpdate,
			SystemHomeWire.Json.NetworkControl);
	}
}
