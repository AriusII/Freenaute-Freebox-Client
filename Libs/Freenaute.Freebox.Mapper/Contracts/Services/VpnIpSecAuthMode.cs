namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNIPSecAuthMode wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNIPSecAuthMode" />
public sealed record VpnIpSecAuthMode
{
    [JsonPropertyName("id_source")] public VpnIpSecIdSource? IdSource { get; init; }

    [JsonPropertyName("id_custom")] public string? IdCustom { get; init; }

    public override string ToString()
    {
        return nameof(VpnIpSecAuthMode);
    }
}
