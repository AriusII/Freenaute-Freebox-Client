namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNWireGuardConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNWireGuardConfig" />
public sealed record VpnWireGuardConfig
{
    [JsonPropertyName("mtu")] public long? Mtu { get; init; }

    public override string ToString()
    {
        return nameof(VpnWireGuardConfig);
    }
}
