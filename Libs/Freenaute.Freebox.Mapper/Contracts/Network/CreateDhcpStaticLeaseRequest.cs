namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpStaticLease.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class CreateDhcpStaticLeaseRequest
{
	/// <summary>Host mac address</summary>
	[JsonPropertyName("mac")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Mac { get; init; }

	/// <summary>an optional comment</summary>
	[JsonPropertyName("comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Comment { get; init; }

	/// <summary>IPv4 to assign to the host</summary>
	[JsonPropertyName("ip")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Ip { get; init; }

	/// <summary>List of dns options to include in DHCP reply</summary>
	[JsonPropertyName("options")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<DhcpOptionWrite[]>))]
	public Optional<DhcpOptionWrite[]> Options { get; init; }
}
