namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiApConfig
{
    /// <summary>band Description 2d4g 2.4 GHz 5g 5 GHz 6g 6 GHz 60g 60 GHz</summary>
    [JsonPropertyName("band")]
    public string? Band { get; init; }

    /// <summary>wanted channel width (in MHz) : 20 MHz 40 MHz 80 MHz 160 MHz</summary>
    [JsonPropertyName("channel_width")]
    public StringOrInteger? ChannelWidth { get; init; }

    /// <summary>wanted primary channel, value of 0 means automatic selection</summary>
    [JsonPropertyName("primary_channel")]
    public long? PrimaryChannel { get; init; }

    /// <summary>wanted secondary channel, value of 0 means automatic selection</summary>
    [JsonPropertyName("secondary_channel")]
    public long? SecondaryChannel { get; init; }

    /// <summary>enable channels that require DFS</summary>
    [JsonPropertyName("dfs_enabled")]
    public bool? DfsEnabled { get; init; }

    /// <summary>wifi ht config</summary>
    [JsonPropertyName("ht")]
    public WifiApHtConfig? Ht { get; init; }

    /// <summary>wifi HE config</summary>
    [JsonPropertyName("he")]
    public WifiApHeConfig? He { get; init; }
}
