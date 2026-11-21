namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiGlobalConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiGlobalConfigurationPatch
{
	/// <summary>is wifi enabled</summary>
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>
	///     mac_filter_state Description disabled mac filter is disabled whitelist mac filter is enabled, using a
	///     whitelist blacklist mac filter is enabled, using a blacklist
	/// </summary>
	[JsonPropertyName("mac_filter_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> MacFilterState { get; init; }
}
