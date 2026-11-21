namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiAp.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiAccessPointPatch
{
	/// <summary>ap configuration</summary>
	[JsonPropertyName("config")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<WifiApConfigurationPatch>))]
	public Optional<WifiApConfigurationPatch> Config { get; init; }
}
