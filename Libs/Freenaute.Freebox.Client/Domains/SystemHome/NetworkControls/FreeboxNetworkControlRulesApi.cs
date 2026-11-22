namespace Freenaute.Freebox.Client.Domains.SystemHome.NetworkControls;

public sealed class FreeboxNetworkControlRulesApi
{
	private readonly string _path;
	private readonly IFreeboxTransport _transport;

	internal FreeboxNetworkControlRulesApi(IFreeboxTransport transport, long id)
	{
		_transport = transport;
		_path = "network_control/" + SystemHomeWire.Id(id) + "/rules";
	}

	public async Task<ImmutableArray<NetworkControlRule>> ListAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, _path, SystemHomeWire.Json.NetworkControlRules, cancellationToken)
			.ConfigureAwait(false);

	public async Task<NetworkControlRule> GetAsync(long ruleId, CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, _path + "/" + SystemHomeWire.Id(ruleId),
			SystemHomeWire.Json.NetworkControlRule, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxSystemHomeCommand<NetworkControlRuleUpdate, NetworkControlRule> Create() =>
		new(_transport, HttpMethod.Post, _path, new NetworkControlRuleUpdate(),
			SystemHomeWire.Json.NetworkControlRuleUpdate, SystemHomeWire.Json.NetworkControlRule);

	public FreeboxSystemHomeCommand<NetworkControlRuleUpdate, NetworkControlRule> Update(long ruleId) =>
		new(_transport, HttpMethod.Put, _path + "/" + SystemHomeWire.Id(ruleId),
			new NetworkControlRuleUpdate(), SystemHomeWire.Json.NetworkControlRuleUpdate,
			SystemHomeWire.Json.NetworkControlRule);

	public async Task DeleteAsync(long ruleId, CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, _path + "/" + SystemHomeWire.Id(ruleId), cancellationToken)
			.ConfigureAwait(false);
}
