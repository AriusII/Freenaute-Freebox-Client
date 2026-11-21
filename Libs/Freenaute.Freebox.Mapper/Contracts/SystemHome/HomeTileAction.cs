namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeTile.action; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeTileAction>))]
public readonly record struct HomeTileAction(string Value) : ISystemHomeWireToken<HomeTileAction>
{
    public bool IsKnown => Value is "tileset" or "graph" or "store" or "store_slider" or "color_picker" or "heat_picker"
        or "intensity_picker" or "none";

    public static HomeTileAction Tileset { get; } = new("tileset");
    public static HomeTileAction Graph { get; } = new("graph");
    public static HomeTileAction Store { get; } = new("store");
    public static HomeTileAction StoreSlider { get; } = new("store_slider");
    public static HomeTileAction ColorPicker { get; } = new("color_picker");
    public static HomeTileAction HeatPicker { get; } = new("heat_picker");
    public static HomeTileAction IntensityPicker { get; } = new("intensity_picker");
    public static HomeTileAction None { get; } = new("none");

    public static HomeTileAction FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeTileAction(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
