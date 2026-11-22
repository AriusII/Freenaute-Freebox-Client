namespace Freenaute.Freebox.Mapper.Contracts.Network.VpnClient;

/// <summary>VPN client status. This API is unstable and may change.</summary>
public sealed class VPNClientStatus
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }
	[JsonPropertyName("active_vpn")] public string? ActiveVpn { get; init; }

	[JsonPropertyName("active_vpn_description")]
	public string? ActiveVpnDescription { get; init; }

	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("last_up")] public long? LastUp { get; init; }
	[JsonPropertyName("last_try")] public long? LastTry { get; init; }
	[JsonPropertyName("next_try")] public long? NextTry { get; init; }
	[JsonPropertyName("last_error")] public string? LastError { get; init; }
	[JsonPropertyName("stats")] public VpnClientStats? Stats { get; init; }
	[JsonPropertyName("ipv4")] public VpnClientIpInfo? Ipv4 { get; init; }
}
