namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpointUi; absent metadata remains
///     nullable.
/// </summary>
public sealed record HomeNodeEndpointUi
{
    // The UI access field is present in the tileset examples.
    [JsonPropertyName("access")] public HomeEndpointAccess? Access { get; init; }
    [JsonPropertyName("display")] public HomeEndpointDisplay? Display { get; init; }
    [JsonPropertyName("icon_url")] public string? IconUrl { get; init; }
    [JsonPropertyName("unit")] public string? Unit { get; init; }
    [JsonPropertyName("icon_color")] public string? IconColor { get; init; }
    [JsonPropertyName("text_color")] public string? TextColor { get; init; }
    [JsonPropertyName("value_color")] public string? ValueColor { get; init; }
    [JsonPropertyName("range")] public ImmutableArray<double>? Range { get; init; }
    [JsonPropertyName("icon_color_range")] public ImmutableArray<string>? IconColorRange { get; init; }
    [JsonPropertyName("text_color_range")] public ImmutableArray<string>? TextColorRange { get; init; }

    [JsonPropertyName("value_color_range")]
    public ImmutableArray<string>? ValueColorRange { get; init; }

    [JsonPropertyName("status_text_range")]
    public ImmutableArray<string>? StatusTextRange { get; init; }
}
