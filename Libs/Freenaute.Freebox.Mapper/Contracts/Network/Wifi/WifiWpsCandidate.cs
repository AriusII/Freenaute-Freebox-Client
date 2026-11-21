namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiWpsCandidate.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiWpsCandidate
{
	/// <summary>bss id</summary>
	[JsonPropertyName("bssid")]
	public string? Bssid { get; init; }

	/// <summary>wifi network name</summary>
	[JsonPropertyName("ssid")]
	public string? Ssid { get; init; }

	/// <summary>bss uuid for wps</summary>
	[JsonPropertyName("bss_uuid")]
	public string? BssUuid { get; init; }

	/// <summary>band Description 2d4g 2.4 GHz 5g 5 GHz 60g 60 GHz</summary>
	[JsonPropertyName("band")]
	public string? Band { get; init; }

	/// <summary>currently configured encryption mode see WifiBssConfig encryption field</summary>
	[JsonPropertyName("encryption")]
	public string? Encryption { get; init; }

	/// <summary>is wps enabled for this bss</summary>
	[JsonPropertyName("wps_enabled")]
	public bool? WpsEnabled { get; init; }

	/// <summary>the current state of the associated ap see WifiBssStatus state</summary>
	[JsonPropertyName("state")]
	public string? State { get; init; }
}
