namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#TemporaryWifiDisable.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class TemporaryWifiDisableRequest
{
	/// <summary>temporary disable duration</summary>
	[JsonPropertyName("duration")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> Duration { get; init; }

	/// <summary>
	///     specify a wifi band to keep active keep Description 2d4g keep only 2,4Ghz band active 5g keep only 5GHz bands
	///     active 6g keep only 6GHz band active
	/// </summary>
	[JsonPropertyName("keep")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Keep { get; init; }
}
