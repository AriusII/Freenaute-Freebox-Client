namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNIPSecAuthMode.id_source; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNIPSecAuthMode.id_source" />
[JsonConverter(typeof(VpnIpSecIdSourceJsonConverter))]
public readonly record struct VpnIpSecIdSource(string Value)
{
    public static VpnIpSecIdSource Custom => new("custom");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
