namespace Freenaute.Freebox.Mapper.Contracts.Network.Ddns;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DDNSStatus.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DdnsStatus
{
	/// <summary>
	///     Status Description disabled Disabled ok Ok wait Updating reqfail Request failed authfail Authentication error
	///     nocredential Invalid credential ipinval Invalid IP hostinval Invalid hostname abuse Blocked because of abuse
	///     dnserror DNS error unavailable Service unavailable nowan Unable to get wan IP unknown Unknown
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>next refresh timestamp</summary>
	[JsonPropertyName("next_refresh")]
	public long? NextRefresh { get; init; }

	/// <summary>last refresh timestamp</summary>
	[JsonPropertyName("last_refresh")]
	public long? LastRefresh { get; init; }

	/// <summary>next retry timestamp</summary>
	[JsonPropertyName("next_retry")]
	public long? NextRetry { get; init; }

	/// <summary>last error timestamp</summary>
	[JsonPropertyName("last_error")]
	public long? LastError { get; init; }
}
