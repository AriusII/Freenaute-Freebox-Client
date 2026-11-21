namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#SystemConfigExpansion; absent metadata remains
///     nullable.
/// </summary>
public sealed record SystemConfigExpansion
{
    [JsonPropertyName("slot")] public long? Slot { get; init; }
    [JsonPropertyName("probe_done")] public bool? ProbeDone { get; init; }
    [JsonPropertyName("present")] public bool? Present { get; init; }
    [JsonPropertyName("supported")] public bool? Supported { get; init; }
    [JsonPropertyName("bundle")] public string? Bundle { get; init; }
    [JsonPropertyName("type")] public SystemExpansionType? Type { get; init; }
}
