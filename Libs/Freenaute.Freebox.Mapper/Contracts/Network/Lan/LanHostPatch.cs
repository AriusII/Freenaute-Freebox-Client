namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHost.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class LanHostPatch
{
	/// <summary>Host primary name (chosen from the list of available names, or manually set by user)</summary>
	[JsonPropertyName("primary_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> PrimaryName { get; init; }

	/// <summary>
	///     Host domain name on the local network (manually set by user, or automatically configured during device
	///     registration). The string must respect the following rules: Must end with ‘.home’ 63 characters long at max Only
	///     alphabetical characters are accepted Digits are accepted provided they are not placed at the beginning of the
	///     string, nor after another dot character. Hyphens and dots are accepted provided they are not placed at the
	///     beginning or the end of the string, nor after or before another dot character. It is also possible to use an empty
	///     string. This special value means no local domain should be registered for this host.
	/// </summary>
	[JsonPropertyName("domain_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> DomainName { get; init; }

	/// <summary>
	///     When possible, the Freebox will try to guess the host_type, but you can manually override this to the correct
	///     value Possible values are: source Description workstation Workstation laptop Laptop smartphone Smartphone tablet
	///     Tablet printer Printer vg_console Video game console television TV nas Nas ip_camera IP Camera ip_phone IP Phone
	///     freebox_player Freebox Player freebox_hd Freebox HD freebox_crystal Freebox Crystal freebox_mini Freebox Mini 4k
	///     freebox_delta Freebox Delta freebox_one Freebox One freebox_wifi Freebox Wi-Fi Pop freebox_pop Freebox Pop
	///     networking_device Networking device multimedia_device Multimedia device car Connected car watch Smartwatch light
	///     Light outlet Connected outlet appliances Household appliances thermostat Thermostat shutter Electric shutter other
	///     Other
	/// </summary>
	[JsonPropertyName("host_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> HostType { get; init; }

	/// <summary>If true the host is always shown even if it has not been active since the Freebox startup</summary>
	[JsonPropertyName("persistent")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Persistent { get; init; }
}
