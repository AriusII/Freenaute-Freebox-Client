namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpointUi.display; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeEndpointDisplay>))]
public readonly record struct HomeEndpointDisplay(string Value) : ISystemHomeWireToken<HomeEndpointDisplay>
{
    public bool IsKnown => Value is "text" or "icon" or "button" or "slider" or "toggle" or "color" or "warning";
    public static HomeEndpointDisplay Text { get; } = new("text");
    public static HomeEndpointDisplay Icon { get; } = new("icon");
    public static HomeEndpointDisplay Button { get; } = new("button");
    public static HomeEndpointDisplay Slider { get; } = new("slider");
    public static HomeEndpointDisplay Toggle { get; } = new("toggle");
    public static HomeEndpointDisplay Color { get; } = new("color");
    public static HomeEndpointDisplay Warning { get; } = new("warning");

    public static HomeEndpointDisplay FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeEndpointDisplay(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
