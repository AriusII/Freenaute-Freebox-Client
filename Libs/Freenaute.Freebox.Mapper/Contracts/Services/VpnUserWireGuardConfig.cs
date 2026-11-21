namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented conf_wireguard wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNUser.conf_wireguard" />
public sealed record VpnUserWireGuardConfig
{
    [JsonPropertyName("keepalive")] public long? Keepalive { get; init; }

    [JsonPropertyName("psk")] public bool? Psk { get; init; }

    public override string ToString()
    {
        return nameof(VpnUserWireGuardConfig);
    }
}
