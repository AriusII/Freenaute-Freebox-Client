namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnServerAuthentication wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNPPTPConfig.allowed_auth" />
public sealed record VpnServerAuthentication
{
    [JsonPropertyName("pap")] public bool? Pap { get; init; }

    [JsonPropertyName("chap")] public bool? Chap { get; init; }

    [JsonPropertyName("mschapv2")] public bool? Mschapv2 { get; init; }

    public override string ToString()
    {
        return nameof(VpnServerAuthentication);
    }
}
