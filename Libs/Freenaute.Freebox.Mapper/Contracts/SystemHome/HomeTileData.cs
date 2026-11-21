namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeTileData; absent metadata remains nullable.</summary>
public sealed record HomeTileData
{
    [JsonPropertyName("category")] public string? Category { get; init; }
    [JsonPropertyName("refresh")] public long? Refresh { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("ep_id")] public long? EpId { get; init; }
    [JsonPropertyName("value_type")] public HomeTileValueType? ValueType { get; init; }
    [JsonPropertyName("value")] public HomeIoValue? Value { get; init; }
    [JsonPropertyName("history")] public string? History { get; init; }
    [JsonPropertyName("ui")] public HomeNodeEndpointUi? Ui { get; init; }
}
