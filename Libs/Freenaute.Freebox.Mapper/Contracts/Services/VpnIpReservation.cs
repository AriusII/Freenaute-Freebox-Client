namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNIpReservation wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn-ip_pool-" />
public sealed record VpnIpReservation
{
    [JsonPropertyName("login")] public string? Login { get; init; }

    [JsonPropertyName("ip")] public string? Ip { get; init; }

    public override string ToString()
    {
        return nameof(VpnIpReservation);
    }
}
