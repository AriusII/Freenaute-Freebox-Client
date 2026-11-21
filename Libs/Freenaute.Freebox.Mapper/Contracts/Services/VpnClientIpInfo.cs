namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnClientIpInfo wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VpnClientIpInfo" />
public sealed record VpnClientIpInfo
{
    [JsonPropertyName("config_valid")] public bool? ConfigValid { get; init; }

    [JsonPropertyName("ip_mask")] public VpnClientIpMask? IpMask { get; init; }

    [JsonPropertyName("domain")] public string? Domain { get; init; }

    [JsonPropertyName("gateway")] public string? Gateway { get; init; }

    [JsonPropertyName("dns")] public string[]? Dns { get; init; }

    [JsonPropertyName("provider")] public VpnIpProvider? Provider { get; init; }

    [JsonPropertyName("dhcp")] public VpnClientDhcpState? Dhcp { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientIpInfo);
    }
}
