namespace Freenaute.Freebox.Mapper.Contracts.Network.VpnClient;

/// <summary>VPN client configuration. This API is unstable and may change.</summary>
public sealed class VPNClientConfig
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("description")] public string? Description { get; init; }
	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("active")] public bool? Active { get; init; }
	[JsonPropertyName("conf_pptp")] public VPNClientConfigPPTP? ConfPptp { get; init; }
}
