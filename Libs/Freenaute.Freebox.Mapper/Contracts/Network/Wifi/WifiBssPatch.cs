namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiBss.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiBssPatch
{
	/// <summary>
	///     if set to True the bss will use the shared parameters stored under shared_bss_params if not the bss will use a
	///     configuration specific to this bss stored under bss_params when you want to edit the bss config you should change
	///     the config values using values from bss_params or shared_bss_params as a source and update use_shared_params
	///     accordingly.
	/// </summary>
	[JsonPropertyName("use_shared_params")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> UseSharedParams { get; init; }

	/// <summary>bss configuration (use this field for editing)</summary>
	[JsonPropertyName("config")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<WifiBssConfigurationPatch>))]
	public Optional<WifiBssConfigurationPatch> Config { get; init; }
}
