namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiNeighbor.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiNeighbor
{
	/// <summary>neighbor bssid</summary>
	[JsonPropertyName("bssid")]
	public string? Bssid { get; init; }

	/// <summary>neighbor ssid</summary>
	[JsonPropertyName("ssid")]
	public string? Ssid { get; init; }

	/// <summary>the band for which the combination can be used band Description 2d4g 2.4 GHz 5g 5 GHz 60g 60 GHz</summary>
	[JsonPropertyName("band")]
	public string? Band { get; init; }

	/// <summary>neighbor channel_width</summary>
	[JsonPropertyName("channel_width")]
	public StringOrInteger? ChannelWidth { get; init; }

	/// <summary>neighbor primary channel</summary>
	[JsonPropertyName("channel")]
	public long? Channel { get; init; }

	/// <summary>neighbor secondary channel (0 for unused)</summary>
	[JsonPropertyName("secondary_channel")]
	public long? SecondaryChannel { get; init; }

	/// <summary>signal attenuation in dB</summary>
	[JsonPropertyName("signal")]
	public long? Signal { get; init; }

	/// <summary>neighbor capabilities</summary>
	[JsonPropertyName("capabilities")]
	public WifiNeighborCap? Capabilities { get; init; }
}
