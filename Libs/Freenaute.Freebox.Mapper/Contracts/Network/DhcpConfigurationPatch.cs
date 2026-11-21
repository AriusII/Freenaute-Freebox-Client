namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class DhcpConfigurationPatch
{
	/// <summary>Enable/Disable the DHCP server</summary>
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>Always assign the same IP to a given host</summary>
	[JsonPropertyName("sticky_assign")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> StickyAssign { get; init; }

	/// <summary>DHCP range start IP</summary>
	[JsonPropertyName("ip_range_start")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> IpRangeStart { get; init; }

	/// <summary>DHCP range end IP</summary>
	[JsonPropertyName("ip_range_end")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> IpRangeEnd { get; init; }

	/// <summary>Always broadcast DHCP responses</summary>
	[JsonPropertyName("always_broadcast")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> AlwaysBroadcast { get; init; }

	/// <summary>Ignore requested address if it is outside of the DHCP range</summary>
	[JsonPropertyName("ignore_out_of_range_hint")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> IgnoreOutOfRangeHint { get; init; }

	/// <summary>Address of the TFTP server used when booting via TFTP.</summary>
	[JsonPropertyName("boot_server")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> BootServer { get; init; }

	/// <summary>Boot file to download from the TFTP server when booting via TFTP.</summary>
	[JsonPropertyName("boot_file")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> BootFile { get; init; }

	/// <summary>List of dns servers to include in DHCP reply</summary>
	[JsonPropertyName("dns")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string[]>))]
	public Optional<string[]> Dns { get; init; }

	/// <summary>List of dns options to include in DHCP reply</summary>
	[JsonPropertyName("options")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<DhcpOptionWrite[]>))]
	public Optional<DhcpOptionWrite[]> Options { get; init; }
}
