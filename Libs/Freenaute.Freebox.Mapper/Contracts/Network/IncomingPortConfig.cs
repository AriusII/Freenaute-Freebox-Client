namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#IncomingPortConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class IncomingPortConfig
{
	/// <summary>
	///     incoming port id id Description http http port for remote access to Freebox OS https https port for tls remote
	///     access to Freebox OS bittorrent-main main bittorrent port for Freebox downloader bittorrent-dht bittorrent port for
	///     DHT openvpn_routed routed openvpn port openvpn_bridge bridged openvpn port ipsec_ike ipsec ikev2 vpn port ipsec_nat
	///     ipsec nat vpn port pptp pptp vpn server port ftp ftp control port for FTP remote access ftp_pasv ftp data port for
	///     FTP remote access
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>is the port binding allowed</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>is the port binding currently active</summary>
	[JsonPropertyName("active")]
	public bool? Active { get; init; }

	/// <summary>ip_proto Description tcp TCP udp UDP tcp_udp both TCP and UDP</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>binding port</summary>
	[JsonPropertyName("in_port")]
	public long? InPort { get; init; }

	/// <summary>
	///     network namespace. The service may be running on a different namespace (for instance if the service uses the
	///     vpn client).
	/// </summary>
	[JsonPropertyName("netns")]
	public string? Netns { get; init; }

	/// <summary>This field indicate the minimum possible value for in_port (see ConnectionStatus ipv4_port_range)</summary>
	[JsonPropertyName("min_port")]
	public long? MinPort { get; init; }

	/// <summary>This field indicate the maximum possible value for in_port (see ConnectionStatus ipv4_port_range)</summary>
	[JsonPropertyName("max_port")]
	public long? MaxPort { get; init; }

	/// <summary>If set to true, the in_port field cannot be changed because of the underlying protocol does not allow it</summary>
	[JsonPropertyName("readonly")]
	public bool? Readonly { get; init; }
}
