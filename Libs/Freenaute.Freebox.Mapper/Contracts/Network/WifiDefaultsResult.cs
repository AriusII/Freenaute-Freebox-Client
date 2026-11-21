namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-default.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiDefaultsResult
{
    [JsonPropertyName("aps")] public WifiDefaultAp[]? Aps { get; init; }

    [JsonPropertyName("bsss")] public WifiDefaultBss[]? Bsss { get; init; }
}
