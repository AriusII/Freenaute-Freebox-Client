namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LteNetwork.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LteNetwork
{
	[JsonPropertyName("pdn_up")] public bool? PdnUp { get; init; }

	[JsonPropertyName("has_ipv6")] public bool? HasIpv6 { get; init; }

	[JsonPropertyName("ipv6_dns")] public string? Ipv6Dns { get; init; }

	[JsonPropertyName("ipv6")] public string? Ipv6 { get; init; }

	[JsonPropertyName("ipv6_netmask")] public string? Ipv6Netmask { get; init; }

	[JsonPropertyName("has_ipv4")] public bool? HasIpv4 { get; init; }

	[JsonPropertyName("ipv4_dns")] public string? Ipv4Dns { get; init; }

	[JsonPropertyName("ipv4")] public string? Ipv4 { get; init; }

	[JsonPropertyName("ipv4_netmask")] public string? Ipv4Netmask { get; init; }
}
