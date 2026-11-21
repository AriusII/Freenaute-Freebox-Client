namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApChannelSurveyData.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiApChannelSurveyData
{
	/// <summary>timestamp at which the survey data was retrieved</summary>
	[JsonPropertyName("timestamp")]
	public long? Timestamp { get; init; }

	/// <summary>percentage of time the channel was sensed busy</summary>
	[JsonPropertyName("busy_percent")]
	public long? BusyPercent { get; init; }

	/// <summary>percentage of time spent sending on the channel</summary>
	[JsonPropertyName("tx_percent")]
	public long? TxPercent { get; init; }

	/// <summary>percentage of time spent receiving Wi-Fi traffic on the channel</summary>
	[JsonPropertyName("rx_percent")]
	public long? RxPercent { get; init; }

	/// <summary>percentage of time spent receiving Wi-Fi traffic for a local BSS</summary>
	[JsonPropertyName("rx_bss_percent")]
	public long? RxBssPercent { get; init; }
}
