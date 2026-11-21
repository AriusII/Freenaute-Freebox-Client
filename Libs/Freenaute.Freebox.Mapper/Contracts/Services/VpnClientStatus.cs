namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNClientStatus wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientStatus" />
public sealed record VpnClientStatus
{
    [JsonPropertyName("enabled")] public bool? Enabled { get; init; }

    [JsonPropertyName("active_vpn")] public string? ActiveVpn { get; init; }

    [JsonPropertyName("active_vpn_description")]
    public string? ActiveVpnDescription { get; init; }

    [JsonPropertyName("type")] public VpnClientType? Type { get; init; }

    [JsonPropertyName("state")] public VpnClientState? State { get; init; }

    [JsonPropertyName("last_up")] public long? LastUp { get; init; }

    [JsonPropertyName("last_try")] public long? LastTry { get; init; }

    [JsonPropertyName("next_try")] public long? NextTry { get; init; }

    [JsonPropertyName("last_error")] public VpnClientError? LastError { get; init; }

    [JsonPropertyName("stats")] public VpnClientStats? Stats { get; init; }

    [JsonPropertyName("IPv4")] public VpnClientIpInfo? IPv4Declared { get; init; }

    [JsonPropertyName("ipv4")] public VpnClientIpInfo? Ipv4Example { get; init; }

    [JsonIgnore] public VpnClientIpInfo? IpV4 => Ipv4Example ?? IPv4Declared;

    public override string ToString()
    {
        return nameof(VpnClientStatus);
    }
}
