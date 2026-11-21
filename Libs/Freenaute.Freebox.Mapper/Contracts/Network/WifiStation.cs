namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiStation.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiStation
{
	/// <summary>station id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>client MAC address</summary>
	[JsonPropertyName("mac")]
	public string? Mac { get; init; }

	/// <summary>bssid on which the client is associated</summary>
	[JsonPropertyName("bssid")]
	public string? Bssid { get; init; }

	/// <summary>client host name</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>client host information</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }

	/// <summary>state Description associated station is associated authenticated station is authenticated</summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }

	/// <summary>inactive duration (in seconds)</summary>
	[JsonPropertyName("inactive")]
	public long? Inactive { get; init; }

	/// <summary>connection duration (in seconds)</summary>
	[JsonPropertyName("conn_duration")]
	public long? ConnDuration { get; init; }

	/// <summary>received bytes (from station to Freebox)</summary>
	[JsonPropertyName("rx_bytes")]
	public long? RxBytes { get; init; }

	/// <summary>transmitted bytes (from Freebox to station)</summary>
	[JsonPropertyName("tx_bytes")]
	public long? TxBytes { get; init; }

	/// <summary>reception data rate (in bytes/s)</summary>
	[JsonPropertyName("tx_rate")]
	public long? TxRate { get; init; }

	/// <summary>transmission data rate (in bytes/s)</summary>
	[JsonPropertyName("rx_rate")]
	public long? RxRate { get; init; }

	/// <summary>signal attenuation (in dB)</summary>
	[JsonPropertyName("signal")]
	public long? Signal { get; init; }

	/// <summary>station flags</summary>
	[JsonPropertyName("flags")]
	public WifiStationFlags? Flags { get; init; }

	/// <summary>last rx stats</summary>
	[JsonPropertyName("last_rx")]
	public WifiStationStats? LastRx { get; init; }

	/// <summary>last tx stats</summary>
	[JsonPropertyName("last_tx")]
	public WifiStationStats? LastTx { get; init; }
}
