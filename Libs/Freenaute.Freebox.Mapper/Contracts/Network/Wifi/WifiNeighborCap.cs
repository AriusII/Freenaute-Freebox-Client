namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiNeighborCap.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiNeighborCap
{
	/// <summary>neighbor uses legacy wifi (802.11a, 802.11b)</summary>
	[JsonPropertyName("legacy")]
	public bool? Legacy { get; init; }

	/// <summary>neighbor supports ht (802.11n)</summary>
	[JsonPropertyName("ht")]
	public bool? Ht { get; init; }

	/// <summary>neighbor supports vht (802.11ac)</summary>
	[JsonPropertyName("vht")]
	public bool? Vht { get; init; }
}
