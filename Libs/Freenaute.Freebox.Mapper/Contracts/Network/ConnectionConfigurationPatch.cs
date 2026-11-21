namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionConfiguration.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class ConnectionConfigurationPatch
{
	/// <summary>should the Freebox respond to external ping requests</summary>
	[JsonPropertyName("ping")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Ping { get; init; }

	/// <summary>enable/disable HTTP remote access</summary>
	[JsonPropertyName("remote_access")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> RemoteAccess { get; init; }

	/// <summary>port number to use for remote HTTP access</summary>
	[JsonPropertyName("remote_access_port")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> RemoteAccessPort { get; init; }

	/// <summary>enable/disable Wake-on-lan proxy</summary>
	[JsonPropertyName("wol")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Wol { get; init; }

	/// <summary>is ads blocking feature enabled</summary>
	[JsonPropertyName("adblock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Adblock { get; init; }

	/// <summary>
	///     if false, user has disabled new token request. New apps can’t request a new token. Apps that already have a
	///     token are still allowed
	/// </summary>
	[JsonPropertyName("allow_token_request")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> AllowTokenRequest { get; init; }

	/// <summary>
	///     Status Description disabled Fully disable SIP ALG direct_media Enable SIP ALG, RTP only allowed between SIP UA
	///     any_media Enable SIP ALG, RTP allowed between any host (dangerous for untrusted hosts)
	/// </summary>
	[JsonPropertyName("sip_alg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> SipAlg { get; init; }
}
