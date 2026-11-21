namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class ConnectionStatus
{
	/// <summary>
	///     State Description going_up connection is initializing up connection is active going_down connection is about
	///     to become inactive down connection is inactive
	/// </summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }

	/// <summary>Type Description ethernet FTTH/ethernet rfc2684 xDSL (unbundled) pppoatm xDSL</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>Media Description ftth FTTH ethernet ethernet xdsl xDSL backup_4g Internet Backup</summary>
	[JsonPropertyName("media")]
	public string? Media { get; init; }

	/// <summary>Freebox IPv4 address NOTE: this field is only available when connection state is up</summary>
	[JsonPropertyName("ipv4")]
	public string? Ipv4 { get; init; }

	/// <summary>Freebox IPv6 address NOTE: this field is only available when connection state is up</summary>
	[JsonPropertyName("ipv6")]
	public string? Ipv6 { get; init; }

	/// <summary>current upload rate in byte/s</summary>
	[JsonPropertyName("rate_up")]
	public long? RateUp { get; init; }

	/// <summary>current download rate in byte/s</summary>
	[JsonPropertyName("rate_down")]
	public long? RateDown { get; init; }

	/// <summary>available upload bandwidth in bit/s</summary>
	[JsonPropertyName("bandwidth_up")]
	public long? BandwidthUp { get; init; }

	/// <summary>available download bandwidth in bit/s</summary>
	[JsonPropertyName("bandwidth_down")]
	public long? BandwidthDown { get; init; }

	/// <summary>total uploaded bytes since last connection</summary>
	[JsonPropertyName("bytes_up")]
	public long? BytesUp { get; init; }

	/// <summary>total downloaded bytes since last connection</summary>
	[JsonPropertyName("bytes_down")]
	public long? BytesDown { get; init; }

	/// <summary>
	///     Some customers share the same IPv4 and each customer is then assigned a port range. The first value is the
	///     first port of the assigned range and the second value is the last port (inclusive). All PortForwardingConfig must
	///     use ports in this range to be effective.
	/// </summary>
	[JsonPropertyName("ipv4_port_range")]
	public long[]? Ipv4PortRange { get; init; }
}
