namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNClientConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNClientConfig" />
public sealed record VpnClientConfig
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    [JsonPropertyName("description")] public string? Description { get; init; }

    [JsonPropertyName("type")] public VpnClientType? Type { get; init; }

    [JsonPropertyName("active")] public bool? Active { get; init; }

    [JsonPropertyName("conf_pptp")] public VpnClientPptpConfig? ConfPptp { get; init; }

    [JsonPropertyName("conf_wireguard")] public VpnClientWireGuardConfig? ConfWireGuard { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientConfig);
    }
}
