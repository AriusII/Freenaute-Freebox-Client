namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ConnectionIpv6Delegation.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class ConnectionIpv6Delegation
{
    /// <summary>IPv6 prefix</summary>
    [JsonPropertyName("prefix")]
    public string? Prefix { get; init; }

    /// <summary>the next hop for the prefix</summary>
    [JsonPropertyName("next_hop")]
    public string? NextHop { get; init; }
}
