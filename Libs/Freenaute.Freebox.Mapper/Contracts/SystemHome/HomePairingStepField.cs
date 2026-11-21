namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#HomePairingStepField; absent metadata remains
///     nullable.
/// </summary>
public sealed record HomePairingStepField
{
    [JsonPropertyName("widget")] public HomePairingWidget? Widget { get; init; }
    [JsonPropertyName("text")] public string? Text { get; init; }
}
