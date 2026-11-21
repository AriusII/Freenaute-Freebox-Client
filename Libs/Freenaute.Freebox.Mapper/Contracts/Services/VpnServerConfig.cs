namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNServerConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNServerConfig" />
public sealed record VpnServerConfig
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    [JsonPropertyName("type")] public VpnServerType? Type { get; init; }

    [JsonPropertyName("enabled")] public bool? Enabled { get; init; }

    [JsonPropertyName("enable_ipv4")] public bool? EnableIpv4 { get; init; }

    [JsonPropertyName("enable_ipv6")] public bool? EnableIpv6 { get; init; }

    [JsonPropertyName("port")] public long? Port { get; init; }

    [JsonPropertyName("min_port")] public long? MinPort { get; init; }

    [JsonPropertyName("max_port")] public long? MaxPort { get; init; }

    [JsonPropertyName("port_ike")] public long? PortIke { get; init; }

    [JsonPropertyName("port_nat")] public long? PortNat { get; init; }

    [JsonPropertyName("conf_pptp")] public VpnPptpConfig? ConfPptp { get; init; }

    [JsonPropertyName("conf_openvpn")] public VpnOpenVpnConfig? ConfOpenVpn { get; init; }

    [JsonPropertyName("conf_ipsec")] public VpnIpSecConfig? ConfIpsec { get; init; }

    [JsonPropertyName("conf_wireguard")] public VpnWireGuardConfig? ConfWireGuard { get; init; }

    [JsonPropertyName("ip_start")] public string? IpStart { get; init; }

    [JsonPropertyName("ip_end")] public string? IpEnd { get; init; }

    [JsonPropertyName("ip6_start")] public string? Ip6Start { get; init; }

    [JsonPropertyName("ip6_end")] public string? Ip6End { get; init; }

    public override string ToString()
    {
        return nameof(VpnServerConfig);
    }
}
