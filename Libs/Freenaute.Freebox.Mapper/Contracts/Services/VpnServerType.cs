namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNServer.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNServer.type" />
[JsonConverter(typeof(VpnServerTypeJsonConverter))]
public readonly record struct VpnServerType(string Value)
{
    public static VpnServerType Ipsec => new("ipsec");
    public static VpnServerType Pptp => new("pptp");
    public static VpnServerType OpenVpn => new("openvpn");
    public static VpnServerType WireGuard => new("wireguard");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
