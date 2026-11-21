namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnClientAuthentication wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfigPPTP.allowed_auth" />
public sealed record VpnClientAuthentication
{
    [JsonPropertyName("eap")] public bool? Eap { get; init; }

    [JsonPropertyName("pap")] public bool? Pap { get; init; }

    [JsonPropertyName("chap")] public bool? Chap { get; init; }

    [JsonPropertyName("mschap")] public bool? Mschap { get; init; }

    [JsonPropertyName("mschapv2")] public bool? Mschapv2 { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientAuthentication);
    }
}
