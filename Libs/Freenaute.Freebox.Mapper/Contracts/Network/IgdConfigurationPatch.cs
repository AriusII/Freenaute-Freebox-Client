namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#UPnPIGDConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class IgdConfigurationPatch
{
	/// <summary>is the UPnP IGD service enabled</summary>
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>UPnP IGD protocol version Supported values are 1 / 2</summary>
	[JsonPropertyName("version")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> Version { get; init; }
}
