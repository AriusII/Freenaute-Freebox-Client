namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHostL3Connectivity.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHostL3Connectivity
{
	/// <summary>Layer 3 address</summary>
	[JsonPropertyName("addr")]
	public string? Addr { get; init; }

	/// <summary>af Description ipv4 IPv4 ipv6 IPv6</summary>
	[JsonPropertyName("af")]
	public string? Af { get; init; }

	/// <summary>is the connection active</summary>
	[JsonPropertyName("active")]
	public bool? Active { get; init; }

	/// <summary>is the connection reachable</summary>
	[JsonPropertyName("reachable")]
	public bool? Reachable { get; init; }

	/// <summary>last activity timestamp</summary>
	[JsonPropertyName("last_activity")]
	public long? LastActivity { get; init; }

	/// <summary>last reachable timestamp</summary>
	[JsonPropertyName("last_time_reachable")]
	public long? LastTimeReachable { get; init; }

	/// <summary>device model if known</summary>
	[JsonPropertyName("model")]
	public string? Model { get; init; }
}
