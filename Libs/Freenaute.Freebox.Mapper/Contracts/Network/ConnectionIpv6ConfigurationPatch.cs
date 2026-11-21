namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionIpv6Configuration.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class ConnectionIpv6ConfigurationPatch
{
    /// <summary>is IPv6 enabled</summary>
    [JsonPropertyName("ipv6_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<bool>))]
    public Optional<bool> Ipv6Enabled { get; init; }

    /// <summary>is IPv6 firewall enabled</summary>
    [JsonPropertyName("ipv6_firewall")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<bool>))]
    public Optional<bool> Ipv6Firewall { get; init; }

    /// <summary>is IPv6 firewall enabled on secondary prefixes</summary>
    [JsonPropertyName("ipv6_prefix_firewall")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<bool>))]
    public Optional<bool> Ipv6PrefixFirewall { get; init; }

    /// <summary>list of IPv6 delegations</summary>
    [JsonPropertyName("delegations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<ConnectionIpv6DelegationPatch[]>))]
    public Optional<ConnectionIpv6DelegationPatch[]> Delegations { get; init; }
}
