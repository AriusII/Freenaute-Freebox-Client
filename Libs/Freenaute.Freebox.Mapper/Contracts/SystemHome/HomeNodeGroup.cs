namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNodeGroup; absent metadata remains nullable.</summary>
public sealed record HomeNodeGroup
{
    [JsonPropertyName("label")] public string? Label { get; init; }
    [JsonPropertyName("icon_url")] public string? IconUrl { get; init; }
}
