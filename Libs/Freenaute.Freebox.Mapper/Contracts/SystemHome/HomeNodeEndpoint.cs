namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpoint; absent metadata remains
///     nullable.
/// </summary>
public sealed record HomeNodeEndpoint
{
    [JsonPropertyName("category")] public string? Category { get; init; }
    [JsonPropertyName("ep_type")] public HomeEndpointKind? EndpointKind { get; init; }
    [JsonPropertyName("id")] public long? Id { get; init; }
    [JsonPropertyName("visibility")] public HomeEndpointVisibility? Visibility { get; init; }
    [JsonPropertyName("access")] public HomeEndpointAccess? Access { get; init; }
}
