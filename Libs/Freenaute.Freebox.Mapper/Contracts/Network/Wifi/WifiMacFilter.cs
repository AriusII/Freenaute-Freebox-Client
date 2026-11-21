namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiMacFilter.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiMacFilter
{
	/// <summary>filter id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>MAC address to filter</summary>
	[JsonPropertyName("mac")]
	public string? Mac { get; init; }

	/// <summary>comment</summary>
	[JsonPropertyName("comment")]
	public string? Comment { get; init; }

	/// <summary>
	///     type Description whitelist if mac_filter is set to whitelist this station will be allowed blacklist if
	///     mac_filter is set to blacklist this station will be rejected
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>host name when available</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>host information when available</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }
}
