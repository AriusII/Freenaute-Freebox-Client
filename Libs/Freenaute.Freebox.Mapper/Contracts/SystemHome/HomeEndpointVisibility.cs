namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpoint.visibility; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeEndpointVisibility>))]
public readonly record struct HomeEndpointVisibility(string Value) : ISystemHomeWireToken<HomeEndpointVisibility>
{
    public bool IsKnown => Value is "internal" or "normal" or "dashboard";
    public static HomeEndpointVisibility Internal { get; } = new("internal");
    public static HomeEndpointVisibility Normal { get; } = new("normal");
    public static HomeEndpointVisibility Dashboard { get; } = new("dashboard");

    public static HomeEndpointVisibility FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeEndpointVisibility(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
