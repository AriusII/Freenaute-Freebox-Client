namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNClientConfig.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfig.type" />
[JsonConverter(typeof(VpnClientTypeJsonConverter))]
public readonly record struct VpnClientType(string Value)
{
    public static VpnClientType Pptp => new("pptp");
    public static VpnClientType OpenVpn => new("openvpn");
    public static VpnClientType WireGuard => new("wireguard");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
