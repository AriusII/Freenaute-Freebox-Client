namespace Freenaute.Freebox.Mapper.Contracts.Network.Vpn;

/// <summary>VPN server configuration. This API is unstable and may change.</summary>
public sealed class VPNServerConfig
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }
	[JsonPropertyName("enable_ipv4")] public bool? EnableIpv4 { get; init; }
	[JsonPropertyName("enable_ipv6")] public bool? EnableIpv6 { get; init; }
	[JsonPropertyName("port")] public long? Port { get; init; }
	[JsonPropertyName("min_port")] public long? MinPort { get; init; }
	[JsonPropertyName("max_port")] public long? MaxPort { get; init; }
	[JsonPropertyName("port_ike")] public long? PortIke { get; init; }
	[JsonPropertyName("port_nat")] public long? PortNat { get; init; }
	[JsonPropertyName("conf_pptp")] public VPNPPTPConfig? ConfPptp { get; init; }
	[JsonPropertyName("conf_openvpn")] public VPNOpenVpnConfig? ConfOpenVpn { get; init; }
	[JsonPropertyName("conf_ipsec")] public VPNIPSecConfig? ConfIpSec { get; init; }
	[JsonPropertyName("ip_start")] public string? IpStart { get; init; }
	[JsonPropertyName("ip_end")] public string? IpEnd { get; init; }
	[JsonPropertyName("ip6_start")] public string? Ip6Start { get; init; }
	[JsonPropertyName("ip6_end")] public string? Ip6End { get; init; }
}
