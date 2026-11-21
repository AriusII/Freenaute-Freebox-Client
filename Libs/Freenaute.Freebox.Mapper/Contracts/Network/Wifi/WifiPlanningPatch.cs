namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiPlanning.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiPlanningPatch
{
	/// <summary>is the planning enabled</summary>
	[JsonPropertyName("use_planning")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> UsePlanning { get; init; }

	/// <summary>
	///     mapping for planning : “on” or “off” mapping[0] is monday at 0:0 mapping[7 * resolution - 1] is sunday last
	///     slot (each slot has a duration of 60 * 24 / resolution minutes)
	/// </summary>
	[JsonPropertyName("mapping")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string[]>))]
	public Optional<string[]> Mapping { get; init; }
}
