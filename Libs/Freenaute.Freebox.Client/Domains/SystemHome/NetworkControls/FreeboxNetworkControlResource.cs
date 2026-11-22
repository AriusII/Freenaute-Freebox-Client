namespace Freenaute.Freebox.Client.Domains.SystemHome.NetworkControls;

public sealed class FreeboxNetworkControlResource
{
	private readonly string _path;
	private readonly IFreeboxTransport _transport;

	internal FreeboxNetworkControlResource(IFreeboxTransport transport, long id)
	{
		_transport = transport;
		_path = "network_control/" + SystemHomeWire.Id(id);
		Rules = new FreeboxNetworkControlRulesApi(transport, id);
	}

	public FreeboxNetworkControlRulesApi Rules { get; }

	public async Task<NetworkControl> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, _path, SystemHomeWire.Json.NetworkControl, cancellationToken)
			.ConfigureAwait(false);

	public FreeboxSystemHomeCommand<NetworkControlUpdate, NetworkControl> Configure()
	{
		return new FreeboxSystemHomeCommand<NetworkControlUpdate, NetworkControl>(_transport, HttpMethod.Put, _path,
			new NetworkControlUpdate(), SystemHomeWire.Json.NetworkControlUpdate,
			SystemHomeWire.Json.NetworkControl);
	}
}
