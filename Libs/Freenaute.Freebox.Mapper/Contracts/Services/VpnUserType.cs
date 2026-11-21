namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNUser.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNUser.type" />
[JsonConverter(typeof(VpnUserTypeJsonConverter))]
public readonly record struct VpnUserType(string Value)
{
    public static VpnUserType Standard => new("standard");
    public static VpnUserType WireGuard => new("wireguard");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
