namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiPlanning.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiPlanning
{
	/// <summary>is the planning enabled</summary>
	[JsonPropertyName("use_planning")]
	public bool? UsePlanning { get; init; }

	/// <summary>planning resolution (number of slots per day)</summary>
	[JsonPropertyName("resolution")]
	public long? Resolution { get; init; }

	/// <summary>
	///     mapping for planning : “on” or “off” mapping[0] is monday at 0:0 mapping[7 * resolution - 1] is sunday last
	///     slot (each slot has a duration of 60 * 24 / resolution minutes)
	/// </summary>
	[JsonPropertyName("mapping")]
	public string[]? Mapping { get; init; }
}
