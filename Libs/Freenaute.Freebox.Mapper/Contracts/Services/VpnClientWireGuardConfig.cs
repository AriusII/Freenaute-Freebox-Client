namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNClientConfigWireGuard wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfigWireGuard" />
public sealed record VpnClientWireGuardConfig
{
    [JsonPropertyName("remote_addr")] public string? RemoteAddr { get; init; }

    [JsonPropertyName("remote_port")] public long? RemotePort { get; init; }

    [JsonPropertyName("remote_public_key")]
    public string? RemotePublicKey { get; init; }

    [JsonPropertyName("remote_preshared_key")]
    public string? RemotePresharedKey { get; init; }

    [JsonPropertyName("local_priv_key")] public string? LocalPrivKey { get; init; }

    [JsonPropertyName("local_addr")] public VpnClientWireGuardAddress[]? LocalAddr { get; init; }

    [JsonPropertyName("dns")] public string[]? Dns { get; init; }

    [JsonPropertyName("mtu")] public long? Mtu { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientWireGuardConfig);
    }
}
