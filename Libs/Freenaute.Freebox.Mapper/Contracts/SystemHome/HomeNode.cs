namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNode; absent metadata remains nullable.</summary>
public sealed record HomeNode
{
    [JsonPropertyName("adapter")] public long? Adapter { get; init; }
    [JsonPropertyName("category")] public string? Category { get; init; }
    [JsonPropertyName("id")] public long? Id { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("show_endpoints")] public ImmutableArray<HomeNodeEndpoint>? ShowEndpoints { get; init; }
    [JsonPropertyName("status")] public HomeNodeStatus? Status { get; init; }
    [JsonPropertyName("type")] public HomeNodeType? Type { get; init; }
}
