namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#HomePairingStep; absent metadata remains
///     nullable.
/// </summary>
public sealed record HomePairingStep
{
    [JsonPropertyName("fields")] public ImmutableArray<HomePairingStepField>? Fields { get; init; }
    [JsonPropertyName("icon_url")] public string? IconUrl { get; init; }
    [JsonPropertyName("pageid")] public long? PageId { get; init; }
    [JsonPropertyName("refresh")] public long? Refresh { get; init; }
    [JsonPropertyName("session")] public long? Session { get; init; }
}
