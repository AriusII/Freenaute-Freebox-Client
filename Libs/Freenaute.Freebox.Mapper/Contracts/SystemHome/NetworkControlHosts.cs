namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>The declared LAN host array and the host-name array shown by the profile examples.</summary>
[JsonConverter(typeof(NetworkControlHostsJsonConverter))]
public sealed class NetworkControlHosts
{
    private NetworkControlHosts(NetworkControlHostsKind kind, ImmutableArray<string> names,
        ImmutableArray<LanHost> hosts)
    {
        Kind = kind;
        Names = names;
        Hosts = hosts;
    }

    public NetworkControlHostsKind Kind { get; }
    public ImmutableArray<string> Names { get; }
    public ImmutableArray<LanHost> Hosts { get; }
    public static NetworkControlHosts Empty { get; } = new(NetworkControlHostsKind.Empty, [], []);

    public static NetworkControlHosts FromNames(IEnumerable<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        var names = values.ToImmutableArray();
        if (names.Any(static value => value is null))
        {
            throw new ArgumentException("Host names cannot be null.", nameof(values));
        }

        return names.IsEmpty ? Empty : new NetworkControlHosts(NetworkControlHostsKind.Names, names, []);
    }

    public static NetworkControlHosts FromHosts(IEnumerable<LanHost> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        var hosts = values.ToImmutableArray();
        if (hosts.Any(static value => value is null))
        {
            throw new ArgumentException("LAN hosts cannot be null.", nameof(values));
        }

        return hosts.IsEmpty ? Empty : new NetworkControlHosts(NetworkControlHostsKind.Hosts, [], hosts);
    }
}
