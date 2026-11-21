namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#Route.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class LanRouteWrite
{
	/// <summary>
	///     Destination network IPv4 prefix in CIDR format (e.g. 192.168.1.0/24). A prefix is considered invalid if it is
	///     a subprefix of any reserved network listed below. Network Description 127.0.0.0/8 Loopback network 169.254.0.0/16
	///     Link-local addresses 224.0.0.0/4 IANA: multicast 192.168.27.0/24 Used for VPN and guest WIFI addresses Only one
	///     enabled route may exist for a given prefix. An exists error will be returned if multiple active routes share the
	///     same prefix.
	/// </summary>
	[JsonPropertyName("prefix")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Prefix { get; init; }

	/// <summary>IP address of the next-hop gateway.</summary>
	[JsonPropertyName("gateway")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Gateway { get; init; }

	/// <summary>If false the route is not added to the routing table.</summary>
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>Optional text describing the route.</summary>
	[JsonPropertyName("description")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Description { get; init; }
}
