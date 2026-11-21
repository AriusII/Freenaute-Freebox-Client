namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNClientConfigPPTP wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfigPPTP" />
public sealed record VpnClientPptpConfig
{
    [JsonPropertyName("remote_host")] public string? RemoteHost { get; init; }

    [JsonPropertyName("username")] public string? Username { get; init; }

    [JsonPropertyName("mppe")] public VpnMppeMode? Mppe { get; init; }

    [JsonPropertyName("allowed_auth")] public VpnClientAuthentication? AllowedAuth { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientPptpConfig);
    }
}
