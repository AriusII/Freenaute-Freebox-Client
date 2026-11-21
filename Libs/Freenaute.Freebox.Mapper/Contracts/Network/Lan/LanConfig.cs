namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanConfig
{
	/// <summary>Freebox Server IPv4 address</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>Freebox Server name</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>Freebox Server DNS name</summary>
	[JsonPropertyName("name_dns")]
	public string? NameDns { get; init; }

	/// <summary>Freebox Server mDNS name</summary>
	[JsonPropertyName("name_mdns")]
	public string? NameMdns { get; init; }

	/// <summary>Freebox Server netbios name</summary>
	[JsonPropertyName("name_netbios")]
	public string? NameNetbios { get; init; }

	/// <summary>
	///     The valid LAN modes are: Type Description router The Freebox acts as a network router bridge The Freebox acts
	///     as a network bridge NOTE: in bridge mode, most of Freebox services are disabled. It is recommended to use the
	///     router mode, and third party apps should not change this setting
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>
	///     Example-only mode key; formal property is type. Both read separately, neither writable until conflict
	///     resolved.
	/// </summary>
	[JsonPropertyName("mode")]
	public string? Mode { get; init; }
}
