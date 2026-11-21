namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiSteeringConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiSteeringConfig
{
	/// <summary>
	///     Wi-Fi steering level. Value Description 0 Wi-Fi steering is disabled 1 Devices are steered when they accept
	///     the change 2 Devices are steered more aggressively
	/// </summary>
	[JsonPropertyName("steering_level")]
	public long? SteeringLevel { get; init; }
}
