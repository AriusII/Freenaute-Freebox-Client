namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionIpv6Configuration.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class ConnectionIpv6Configuration
{
    /// <summary>is IPv6 enabled</summary>
    [JsonPropertyName("ipv6_enabled")]
    public bool? Ipv6Enabled { get; init; }

    /// <summary>is IPv6 firewall enabled</summary>
    [JsonPropertyName("ipv6_firewall")]
    public bool? Ipv6Firewall { get; init; }

    /// <summary>is IPv6 firewall enabled on secondary prefixes</summary>
    [JsonPropertyName("ipv6_prefix_firewall")]
    public bool? Ipv6PrefixFirewall { get; init; }

    /// <summary>Freebox IPv6 link local address</summary>
    [JsonPropertyName("ipv6ll")]
    public string? Ipv6ll { get; init; }

    /// <summary>list of IPv6 delegations</summary>
    [JsonPropertyName("delegations")]
    public ConnectionIpv6Delegation[]? Delegations { get; init; }
}
