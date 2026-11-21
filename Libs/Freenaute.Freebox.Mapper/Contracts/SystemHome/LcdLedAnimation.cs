namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#LcdConfig.led_strip_animation; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<LcdLedAnimation>))]
public readonly record struct LcdLedAnimation(string Value) : ISystemHomeWireToken<LcdLedAnimation>
{
    public static LcdLedAnimation FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new LcdLedAnimation(value);
    }

    // Supported animation values are advertised dynamically by the device.
    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
