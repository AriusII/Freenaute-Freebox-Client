namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeTile; absent metadata remains nullable.</summary>
public sealed record HomeTile
{
    [JsonPropertyName("ep_type")] public HomeEndpointKind? EndpointKind { get; init; }
    [JsonPropertyName("node_id")] public long? NodeId { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("action")] public HomeTileAction? Action { get; init; }
    [JsonPropertyName("type")] public HomeTileType? Type { get; init; }
    [JsonPropertyName("group")] public HomeNodeGroup? Group { get; init; }
    [JsonPropertyName("data")] public ImmutableArray<HomeTileData>? Data { get; init; }
}
