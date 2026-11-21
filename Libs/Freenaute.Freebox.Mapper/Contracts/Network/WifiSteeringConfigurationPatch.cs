namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiSteeringConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiSteeringConfigurationPatch
{
	/// <summary>
	///     Wi-Fi steering level. Value Description 0 Wi-Fi steering is disabled 1 Devices are steered when they accept
	///     the change 2 Devices are steered more aggressively
	/// </summary>
	[JsonPropertyName("steering_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> SteeringLevel { get; init; }
}
