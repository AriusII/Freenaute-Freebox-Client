namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnClientDhcpState wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-status" />
public sealed record VpnClientDhcpState
{
    [JsonPropertyName("state")] public string? State { get; init; }

    [JsonPropertyName("renew_remaining")] public long? RenewRemaining { get; init; }

    [JsonPropertyName("lease_remaining")] public long? LeaseRemaining { get; init; }

    [JsonPropertyName("lease_time")] public long? LeaseTime { get; init; }

    [JsonPropertyName("rebind_remaining")] public long? RebindRemaining { get; init; }

    [JsonPropertyName("server_id")] public long? ServerId { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientDhcpState);
    }
}
