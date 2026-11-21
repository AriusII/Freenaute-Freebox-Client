namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNPPTPConfig.mppe; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNPPTPConfig.mppe" />
[JsonConverter(typeof(VpnMppeModeJsonConverter))]
public readonly record struct VpnMppeMode(string Value)
{
    public static VpnMppeMode Disable => new("disable");
    public static VpnMppeMode Require => new("require");
    public static VpnMppeMode Require128 => new("require_128");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
