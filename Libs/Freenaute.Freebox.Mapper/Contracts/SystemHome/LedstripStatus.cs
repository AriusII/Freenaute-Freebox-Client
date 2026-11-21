namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#LedstripStatus; absent metadata remains
///     nullable.
/// </summary>
public sealed record LedstripStatus
{
    [JsonPropertyName("use_planning")] public bool? UsePlanning { get; init; }
    [JsonPropertyName("next_change")] public long? NextChange { get; init; }
}
