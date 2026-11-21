namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VpnClientIpInfo.provider; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VpnClientIpInfo.provider" />
[JsonConverter(typeof(VpnIpProviderJsonConverter))]
public readonly record struct VpnIpProvider(string Value)
{
    public static VpnIpProvider None => new("none");
    public static VpnIpProvider Static => new("static");
    public static VpnIpProvider Ppp => new("ppp");
    public static VpnIpProvider Dhcp => new("dhcp");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
