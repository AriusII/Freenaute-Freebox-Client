namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKeyConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiCustomKeyConfigurationPatch
{
	/// <summary>The name of the dedicated wifi network</summary>
	[JsonPropertyName("ssid")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Ssid { get; init; }
}
