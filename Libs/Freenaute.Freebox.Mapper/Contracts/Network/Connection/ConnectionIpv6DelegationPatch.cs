namespace Freenaute.Freebox.Mapper.Contracts.Network.Connection;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionIpv6Delegation.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class ConnectionIpv6DelegationPatch
{
	/// <summary>IPv6 prefix</summary>
	[JsonPropertyName("prefix")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Prefix { get; init; }

	/// <summary>the next hop for the prefix</summary>
	[JsonPropertyName("next_hop")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> NextHop { get; init; }
}
