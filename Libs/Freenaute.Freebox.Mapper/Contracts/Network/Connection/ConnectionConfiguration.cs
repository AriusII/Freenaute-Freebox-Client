namespace Freenaute.Freebox.Mapper.Contracts.Network.Connection;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionConfiguration.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class ConnectionConfiguration
{
	/// <summary>should the Freebox respond to external ping requests</summary>
	[JsonPropertyName("ping")]
	public bool? Ping { get; init; }

	/// <summary>is the admin password secure enough to enable remote access</summary>
	[JsonPropertyName("is_secure_pass")]
	public bool? IsSecurePass { get; init; }

	/// <summary>enable/disable HTTP remote access</summary>
	[JsonPropertyName("remote_access")]
	public bool? RemoteAccess { get; init; }

	/// <summary>port number to use for remote HTTP access</summary>
	[JsonPropertyName("remote_access_port")]
	public long? RemoteAccessPort { get; init; }

	/// <summary>This field indicate the minimum possible value for remote_access_port (see ConnectionStatus ipv4_port_range)</summary>
	[JsonPropertyName("remote_access_min_port")]
	public long? RemoteAccessMinPort { get; init; }

	/// <summary>This field indicate the maximum possible value for remote_access_port (see ConnectionStatus ipv4_port_range)</summary>
	[JsonPropertyName("remote_access_max_port")]
	public long? RemoteAccessMaxPort { get; init; }

	/// <summary>IPv4 to use for remote access (can be missing if connection is down)</summary>
	[JsonPropertyName("remote_access_ip")]
	public string? RemoteAccessIp { get; init; }

	/// <summary>is remote access enabled for apps, or share link</summary>
	[JsonPropertyName("api_remote_access")]
	public bool? ApiRemoteAccess { get; init; }

	/// <summary>enable/disable Wake-on-lan proxy</summary>
	[JsonPropertyName("wol")]
	public bool? Wol { get; init; }

	/// <summary>is ads blocking feature enabled</summary>
	[JsonPropertyName("adblock")]
	public bool? Adblock { get; init; }

	/// <summary>if set to true adblock setting has never been set by the user</summary>
	[JsonPropertyName("adblock_not_set")]
	public bool? AdblockNotSet { get; init; }

	/// <summary>
	///     if false, user has disabled new token request. New apps can’t request a new token. Apps that already have a
	///     token are still allowed
	/// </summary>
	[JsonPropertyName("allow_token_request")]
	public bool? AllowTokenRequest { get; init; }

	/// <summary>
	///     Status Description disabled Fully disable SIP ALG direct_media Enable SIP ALG, RTP only allowed between SIP UA
	///     any_media Enable SIP ALG, RTP allowed between any host (dangerous for untrusted hosts)
	/// </summary>
	[JsonPropertyName("sip_alg")]
	public string? SipAlg { get; init; }
}
