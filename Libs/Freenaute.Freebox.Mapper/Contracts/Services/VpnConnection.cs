namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNConnection wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNConnection" />
public sealed record VpnConnection
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    [JsonPropertyName("vpn")] public string? Vpn { get; init; }

    [JsonPropertyName("user")] public string? User { get; init; }

    [JsonPropertyName("authenticated")] public bool? Authenticated { get; init; }

    [JsonPropertyName("auth_time")] public long? AuthTime { get; init; }

    [JsonPropertyName("src_ip")] public string? SrcIp { get; init; }

    [JsonPropertyName("src_port")] public long? SrcPort { get; init; }

    [JsonPropertyName("local_ip")] public StringOrInteger? LocalIp { get; init; }

    [JsonPropertyName("rx_bytes")] public long? RxBytes { get; init; }

    [JsonPropertyName("tx_bytes")] public long? TxBytes { get; init; }

    public override string ToString()
    {
        return nameof(VpnConnection);
    }
}
