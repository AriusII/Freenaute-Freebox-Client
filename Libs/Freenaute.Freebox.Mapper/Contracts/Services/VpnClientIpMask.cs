namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnClientIpMask wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-vpn_client-status" />
public sealed record VpnClientIpMask
{
    [JsonPropertyName("ip")] public string? Ip { get; init; }

    [JsonPropertyName("mask")] public string? Mask { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientIpMask);
    }
}
