namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpoint.access; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeEndpointAccess>))]
public readonly record struct HomeEndpointAccess(string Value) : ISystemHomeWireToken<HomeEndpointAccess>
{
    public bool IsKnown => Value is "r" or "w" or "rw";
    public static HomeEndpointAccess R { get; } = new("r");
    public static HomeEndpointAccess W { get; } = new("w");
    public static HomeEndpointAccess Rw { get; } = new("rw");

    public static HomeEndpointAccess FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeEndpointAccess(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
