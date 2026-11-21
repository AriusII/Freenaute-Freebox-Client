namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHost.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHost
{
	/// <summary>Host id (unique on this interface)</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>Host primary name (chosen from the list of available names, or manually set by user)</summary>
	[JsonPropertyName("primary_name")]
	public string? PrimaryName { get; init; }

	/// <summary>
	///     Host domain name on the local network (manually set by user, or automatically configured during device
	///     registration). The string must respect the following rules: Must end with ‘.home’ 63 characters long at max Only
	///     alphabetical characters are accepted Digits are accepted provided they are not placed at the beginning of the
	///     string, nor after another dot character. Hyphens and dots are accepted provided they are not placed at the
	///     beginning or the end of the string, nor after or before another dot character. It is also possible to use an empty
	///     string. This special value means no local domain should be registered for this host.
	/// </summary>
	[JsonPropertyName("domain_name")]
	public string? DomainName { get; init; }

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
	public string? HostType { get; init; }

	/// <summary>If true the primary name has been set manually</summary>
	[JsonPropertyName("primary_name_manual")]
	public bool? PrimaryNameManual { get; init; }

	/// <summary>Layer 2 network id and its type</summary>
	[JsonPropertyName("l2ident")]
	[JsonConverter(typeof(ObjectOrArrayJsonConverter<LanHostL2Ident>))]
	public ObjectOrArray<LanHostL2Ident>? L2Ident { get; init; }

	/// <summary>Host vendor name (from the mac address)</summary>
	[JsonPropertyName("vendor_name")]
	public string? VendorName { get; init; }

	/// <summary>If true the host is always shown even if it has not been active since the Freebox startup</summary>
	[JsonPropertyName("persistent")]
	public bool? Persistent { get; init; }

	/// <summary>If true the host can receive traffic from the Freebox</summary>
	[JsonPropertyName("reachable")]
	public bool? Reachable { get; init; }

	/// <summary>Last time the host was reached</summary>
	[JsonPropertyName("last_time_reachable")]
	public long? LastTimeReachable { get; init; }

	/// <summary>If true the host sends traffic to the Freebox</summary>
	[JsonPropertyName("active")]
	public bool? Active { get; init; }

	/// <summary>Last time the host sent traffic</summary>
	[JsonPropertyName("last_activity")]
	public long? LastActivity { get; init; }

	/// <summary>First time the host sent traffic, or 0 (Unix Epoch) if it wasn’t seen before this field was added.</summary>
	[JsonPropertyName("first_activity")]
	public long? FirstActivity { get; init; }

	/// <summary>List of available names, and their source</summary>
	[JsonPropertyName("names")]
	public LanHostName[]? Names { get; init; }

	/// <summary>List of available layer 3 network connections</summary>
	[JsonPropertyName("l3connectivities")]
	public LanHostL3Connectivity[]? L3Connectivities { get; init; }

	/// <summary>If device is associated with a profile, contains profile summary.</summary>
	[JsonPropertyName("network_control")]
	public LanHostNetworkControl? NetworkControl { get; init; }

	/// <summary>Contains detailed information that could be gathered about the device.</summary>
	[JsonPropertyName("info")]
	public Dictionary<string, JsonElement>? Info { get; init; }

	[JsonPropertyName("interface")] public string? Interface { get; init; }
}
