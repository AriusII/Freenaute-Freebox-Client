namespace Freenaute.Freebox.Mapper.Contracts.Network.Dhcp;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DhcpStaticLease.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DhcpStaticLease
{
	/// <summary>DHCP static lease object id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>Host mac address</summary>
	[JsonPropertyName("mac")]
	public string? Mac { get; init; }

	/// <summary>an optional comment</summary>
	[JsonPropertyName("comment")]
	public string? Comment { get; init; }

	/// <summary>hostname matching the mac address</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>IPv4 to assign to the host</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>LAN host information from LAN browser (refer to LanHost documentation)</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }

	/// <summary>List of dns options to include in DHCP reply</summary>
	[JsonPropertyName("options")]
	public DhcpOption[]? Options { get; init; }
}
