namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApCapabilities.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiApCapabilities
{
    /// <summary>map of capabilities in 2.4 GHz band</summary>
    [JsonPropertyName("2d4g")]
    public WifiBandCapabilities? Band2D4G { get; init; }

    /// <summary>map of capabilities in 5 GHz band</summary>
    [JsonPropertyName("5g")]
    public WifiBandCapabilities? Band5G { get; init; }

    /// <summary>map of capabilities in 6 GHz band</summary>
    [JsonPropertyName("6g")]
    public WifiBandCapabilities? Band6G { get; init; }

    /// <summary>map of capabilities in 60 GHz band</summary>
    [JsonPropertyName("60g")]
    public WifiBandCapabilities? Band60G { get; init; }
}
