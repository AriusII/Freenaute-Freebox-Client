namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNodeType; absent metadata remains nullable.</summary>
public sealed record HomeNodeType
{
    [JsonPropertyName("icon")] public string? Icon { get; init; }
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("physical")] public bool? Physical { get; init; }
}
