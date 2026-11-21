namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpoint.ep_type; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeEndpointKind>))]
public readonly record struct HomeEndpointKind(string Value) : ISystemHomeWireToken<HomeEndpointKind>
{
    public bool IsKnown => Value is "signal" or "slot";
    public static HomeEndpointKind Signal { get; } = new("signal");
    public static HomeEndpointKind Slot { get; } = new("slot");

    public static HomeEndpointKind FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeEndpointKind(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
