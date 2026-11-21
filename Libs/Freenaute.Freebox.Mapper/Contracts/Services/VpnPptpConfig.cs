namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNPPTPConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNPPTPConfig" />
public sealed record VpnPptpConfig
{
    [JsonPropertyName("mppe")] public VpnMppeMode? Mppe { get; init; }

    [JsonPropertyName("allowed_auth")] public VpnServerAuthentication? AllowedAuth { get; init; }

    public override string ToString()
    {
        return nameof(VpnPptpConfig);
    }
}
