namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxNetworkControlRulesApi
{
	private readonly string path;
	private readonly IFreeboxTransport transport;

	internal FreeboxNetworkControlRulesApi(IFreeboxTransport transport, long id)
	{
		this.transport = transport;
		path = "network_control/" + SystemHomeWire.Id(id) + "/rules";
	}

	public Task<ImmutableArray<NetworkControlRule>> ListAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Get, path, SystemHomeWire.Json.NetworkControlRules, cancellationToken);

	public Task<NetworkControlRule> GetAsync(long ruleId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, path + "/" + SystemHomeWire.Id(ruleId),
			SystemHomeWire.Json.NetworkControlRule, cancellationToken);
	}

	public Task DeleteAsync(long ruleId, CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Delete, path + "/" + SystemHomeWire.Id(ruleId), cancellationToken);
}
