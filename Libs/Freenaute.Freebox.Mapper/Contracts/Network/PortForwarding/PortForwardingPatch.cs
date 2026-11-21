namespace Freenaute.Freebox.Mapper.Contracts.Network.PortForwarding;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#PortForwardingConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class PortForwardingPatch
{
	/// <summary>is forwarding enabled</summary>
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>ip_proto Description tcp TCP udp UDP</summary>
	[JsonPropertyName("ip_proto")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> IpProto { get; init; }

	/// <summary>forwarding range start</summary>
	[JsonPropertyName("wan_port_start")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<StringOrInteger>))]
	public Optional<StringOrInteger> WanPortStart { get; init; }

	/// <summary>forwarding range end</summary>
	[JsonPropertyName("wan_port_end")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> WanPortEnd { get; init; }

	/// <summary>forwarding target on LAN</summary>
	[JsonPropertyName("lan_ip")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> LanIp { get; init; }

	/// <summary>forwarding target start port on LAN, (last port is lan_port + wan_port_end - wan_port_start)</summary>
	[JsonPropertyName("lan_port")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> LanPort { get; init; }

	/// <summary>
	///     if src_ip == 0.0.0.0 this rule will apply to any src ip otherwise it will only apply to the specified ip
	///     address
	/// </summary>
	[JsonPropertyName("src_ip")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> SrcIp { get; init; }

	/// <summary>comment</summary>
	[JsonPropertyName("comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Comment { get; init; }
}
