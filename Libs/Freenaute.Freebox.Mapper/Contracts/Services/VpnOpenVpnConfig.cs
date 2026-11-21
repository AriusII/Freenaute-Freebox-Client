namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNOpenVpnConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNOpenVpnConfig" />
public sealed record VpnOpenVpnConfig
{
    [JsonPropertyName("cipher")] public VpnCipher? Cipher { get; init; }

    [JsonPropertyName("disable_fragment")] public bool? DisableFragment { get; init; }

    [JsonPropertyName("use_tcp")] public bool? UseTcp { get; init; }

    public override string ToString()
    {
        return nameof(VpnOpenVpnConfig);
    }
}
