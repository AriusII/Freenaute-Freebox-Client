namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiStationStats.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiStationStats
{
	/// <summary>physical link rate (in 1/10th of MBit/s), -1 if unknown</summary>
	[JsonPropertyName("bitrate")]
	public long? Bitrate { get; init; }

	/// <summary>current link mcs, -1 if not used</summary>
	[JsonPropertyName("mcs")]
	public long? Mcs { get; init; }

	/// <summary>current link vht mcs, -1 if not used</summary>
	[JsonPropertyName("vht_mcs")]
	public long? VhtMcs { get; init; }

	/// <summary>current channel width</summary>
	[JsonPropertyName("width")]
	public string? Width { get; init; }

	/// <summary>is shortgi enabled</summary>
	[JsonPropertyName("shortgi")]
	public bool? Shortgi { get; init; }
}
