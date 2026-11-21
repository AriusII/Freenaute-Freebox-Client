namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeTile.type; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeTileType>))]
public readonly record struct HomeTileType(string Value) : ISystemHomeWireToken<HomeTileType>
{
    public bool IsKnown => Value is "action" or "info" or "light" or "alarm_sensor" or "alarm_control" or "camera";
    public static HomeTileType Action { get; } = new("action");
    public static HomeTileType Info { get; } = new("info");
    public static HomeTileType Light { get; } = new("light");
    public static HomeTileType AlarmSensor { get; } = new("alarm_sensor");
    public static HomeTileType AlarmControl { get; } = new("alarm_control");
    public static HomeTileType Camera { get; } = new("camera");

    public static HomeTileType FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeTileType(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
