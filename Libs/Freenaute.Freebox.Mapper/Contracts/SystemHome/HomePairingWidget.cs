namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomePairingStepField.widget; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomePairingWidget>))]
public readonly record struct HomePairingWidget(string Value) : ISystemHomeWireToken<HomePairingWidget>
{
    public bool IsKnown => Value is "label" or "select" or "button" or "display_qrcode" or "input" or "checkbox"
        or "progress" or "bar_button_left" or "bar_button_right";

    public static HomePairingWidget Label { get; } = new("label");
    public static HomePairingWidget Select { get; } = new("select");
    public static HomePairingWidget Button { get; } = new("button");
    public static HomePairingWidget DisplayQrcode { get; } = new("display_qrcode");
    public static HomePairingWidget Input { get; } = new("input");
    public static HomePairingWidget Checkbox { get; } = new("checkbox");
    public static HomePairingWidget Progress { get; } = new("progress");
    public static HomePairingWidget BarButtonLeft { get; } = new("bar_button_left");
    public static HomePairingWidget BarButtonRight { get; } = new("bar_button_right");

    public static HomePairingWidget FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomePairingWidget(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
