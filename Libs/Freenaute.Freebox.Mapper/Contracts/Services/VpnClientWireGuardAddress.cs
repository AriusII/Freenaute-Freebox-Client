namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNClientConfigWireGuardIP wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfigWireGuardIP" />
public sealed record VpnClientWireGuardAddress
{
    [JsonPropertyName("ip")] public string? Ip { get; init; }

    [JsonPropertyName("len")] public long? Len { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientWireGuardAddress);
    }
}
