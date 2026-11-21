namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHostL2Ident.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHostL2Ident
{
	/// <summary>Layer 2 id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>
	///     Type of layer 2 address source Description dhcp DHCP netbios Netbios mdns mDNS hostname mdns_srv mDNS service
	///     upnp UPnP wsd WS-Discovery
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }
}
