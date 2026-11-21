namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNIPSecConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNIPSecConfig" />
public sealed record VpnIpSecConfig
{
    [JsonPropertyName("ike_version")] public long? IkeVersion { get; init; }

    [JsonPropertyName("auth_modes")] public VpnIpSecAuthModes? AuthModes { get; init; }

    public override string ToString()
    {
        return nameof(VpnIpSecConfig);
    }
}
