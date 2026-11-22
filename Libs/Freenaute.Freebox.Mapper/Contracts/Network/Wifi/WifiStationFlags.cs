namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiStationFlags.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
/// <remarks><b>UNSTABLE API:</b> this contract is marked unstable in the Freebox API documentation.</remarks>
public sealed class WifiStationFlags
{
	/// <summary>does station uses legacy wifi (802.11a, 802.11b)</summary>
	[JsonPropertyName("legacy")]
	public bool? Legacy { get; init; }

	/// <summary>does station support ht (802.11n)</summary>
	[JsonPropertyName("ht")]
	public bool? Ht { get; init; }

	/// <summary>does station support vht (802.11ac)</summary>
	[JsonPropertyName("vht")]
	public bool? Vht { get; init; }

	/// <summary>does station support he (802.11ax)</summary>
	[JsonPropertyName("he")]
	public bool? He { get; init; }

	/// <summary>is the station authenticated</summary>
	[JsonPropertyName("authorized")]
	public bool? Authorized { get; init; }
}
