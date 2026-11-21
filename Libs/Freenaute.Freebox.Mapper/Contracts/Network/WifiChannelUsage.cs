namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiChannelUsage.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiChannelUsage
{
    /// <summary>channel number</summary>
    [JsonPropertyName("channel")]
    public long? Channel { get; init; }

    /// <summary>band Description 2d4g 2.4 GHz 5g 5 GHz 60g 60 GHz</summary>
    [JsonPropertyName("band")]
    public string? Band { get; init; }

    /// <summary>noise level on channel in dB</summary>
    [JsonPropertyName("noise_level")]
    public long? NoiseLevel { get; init; }

    /// <summary>rx channel busy time percentage</summary>
    [JsonPropertyName("rx_busy_percent")]
    public long? RxBusyPercent { get; init; }
}
