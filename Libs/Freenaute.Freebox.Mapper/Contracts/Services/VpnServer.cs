namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VPNServer wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNServer" />
public sealed record VpnServer
{
    [JsonPropertyName("name")] public string? Name { get; init; }

    [JsonPropertyName("type")] public VpnServerType? Type { get; init; }

    [JsonPropertyName("state")] public VpnServerState? State { get; init; }

    [JsonPropertyName("connection_count")] public long? ConnectionCount { get; init; }

    [JsonPropertyName("auth_connection_count")]
    public long? AuthConnectionCount { get; init; }

    public override string ToString()
    {
        return nameof(VpnServer);
    }
}
