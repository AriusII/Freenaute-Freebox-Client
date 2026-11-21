namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeAdapter; absent metadata remains nullable.</summary>
public sealed record HomeAdapter
{
    [JsonPropertyName("id")] public long? Id { get; init; }
    [JsonPropertyName("icon_url")] public string? IconUrl { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("status")] public HomeAdapterStatus? Status { get; init; }
    [JsonPropertyName("type")] public HomeAdapterType? Type { get; init; }
    [JsonPropertyName("props")] public ImmutableDictionary<string, JsonElement>? Props { get; init; }
}
