namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#PortForwardingConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class PortForwardingConfig
{
	/// <summary>forwarding id</summary>
	[JsonPropertyName("id")]
	public long? Id { get; init; }

	/// <summary>is forwarding enabled</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>ip_proto Description tcp TCP udp UDP</summary>
	[JsonPropertyName("ip_proto")]
	public string? IpProto { get; init; }

	/// <summary>forwarding range start</summary>
	[JsonPropertyName("wan_port_start")]
	public StringOrInteger? WanPortStart { get; init; }

	/// <summary>forwarding range end</summary>
	[JsonPropertyName("wan_port_end")]
	public long? WanPortEnd { get; init; }

	/// <summary>forwarding target on LAN</summary>
	[JsonPropertyName("lan_ip")]
	public string? LanIp { get; init; }

	/// <summary>forwarding target start port on LAN, (last port is lan_port + wan_port_end - wan_port_start)</summary>
	[JsonPropertyName("lan_port")]
	public long? LanPort { get; init; }

	/// <summary>forwarding target host name</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>forwarding target host information (see: LanHost )</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }

	/// <summary>
	///     if src_ip == 0.0.0.0 this rule will apply to any src ip otherwise it will only apply to the specified ip
	///     address
	/// </summary>
	[JsonPropertyName("src_ip")]
	public string? SrcIp { get; init; }

	/// <summary>comment</summary>
	[JsonPropertyName("comment")]
	public string? Comment { get; init; }
}
