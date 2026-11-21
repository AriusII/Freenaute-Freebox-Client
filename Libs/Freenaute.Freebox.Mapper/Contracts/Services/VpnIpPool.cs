namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNIpPool wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-ip_pool-" />
public sealed record VpnIpPool
{
    [JsonPropertyName("ip_start")] public string? IpStart { get; init; }

    [JsonPropertyName("ip_end")] public string? IpEnd { get; init; }

    [JsonPropertyName("reservations")] public VpnIpReservation[]? Reservations { get; init; }

    public override string ToString()
    {
        return nameof(VpnIpPool);
    }
}
