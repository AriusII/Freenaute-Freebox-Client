namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v8-lan-browser-types-.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHostTypeDescriptor
{
    [JsonPropertyName("icon")] public string? Icon { get; init; }

    [JsonPropertyName("type")] public string? Type { get; init; }

    [JsonPropertyName("name")] public string? Name { get; init; }

    [JsonPropertyName("category")] public string? Category { get; init; }
}
