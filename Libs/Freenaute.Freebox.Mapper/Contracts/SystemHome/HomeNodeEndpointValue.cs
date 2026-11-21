namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpointValue; absent metadata remains
///     nullable.
/// </summary>
public sealed record HomeNodeEndpointValue
{
    [JsonPropertyName("value")] public HomeIoValue? Value { get; init; }
    [JsonPropertyName("unit")] public string? Unit { get; init; }
    [JsonPropertyName("refresh")] public long? Refresh { get; init; }
    [JsonPropertyName("value_type")] public HomeEndpointValueType? ValueType { get; init; }
}
