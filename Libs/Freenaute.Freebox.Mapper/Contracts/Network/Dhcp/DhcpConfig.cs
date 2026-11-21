namespace Freenaute.Freebox.Mapper.Contracts.Network.Dhcp;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DhcpConfig
{
	/// <summary>Enable/Disable the DHCP server</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>Always assign the same IP to a given host</summary>
	[JsonPropertyName("sticky_assign")]
	public bool? StickyAssign { get; init; }

	/// <summary>Gateway IP address</summary>
	[JsonPropertyName("gateway")]
	public string? Gateway { get; init; }

	/// <summary>Gateway subnet netmask</summary>
	[JsonPropertyName("netmask")]
	public string? Netmask { get; init; }

	/// <summary>DHCP range start IP</summary>
	[JsonPropertyName("ip_range_start")]
	public string? IpRangeStart { get; init; }

	/// <summary>DHCP range end IP</summary>
	[JsonPropertyName("ip_range_end")]
	public string? IpRangeEnd { get; init; }

	/// <summary>Always broadcast DHCP responses</summary>
	[JsonPropertyName("always_broadcast")]
	public bool? AlwaysBroadcast { get; init; }

	/// <summary>Ignore requested address if it is outside of the DHCP range</summary>
	[JsonPropertyName("ignore_out_of_range_hint")]
	public bool? IgnoreOutOfRangeHint { get; init; }

	/// <summary>Address of the TFTP server used when booting via TFTP.</summary>
	[JsonPropertyName("boot_server")]
	public string? BootServer { get; init; }

	/// <summary>Boot file to download from the TFTP server when booting via TFTP.</summary>
	[JsonPropertyName("boot_file")]
	public string? BootFile { get; init; }

	/// <summary>List of dns servers to include in DHCP reply</summary>
	[JsonPropertyName("dns")]
	public string[]? Dns { get; init; }

	/// <summary>List of dns options to include in DHCP reply</summary>
	[JsonPropertyName("options")]
	public DhcpOption[]? Options { get; init; }
}
