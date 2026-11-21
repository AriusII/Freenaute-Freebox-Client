namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKeyHost.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiCustomKeyHost
{
	/// <summary>host name</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>optional host information from Lan Browser (if available)</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }
}
