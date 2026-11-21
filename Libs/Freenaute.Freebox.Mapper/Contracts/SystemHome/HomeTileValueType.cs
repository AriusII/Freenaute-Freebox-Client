namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeTileData.value_type; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeTileValueType>))]
public readonly record struct HomeTileValueType(string Value) : ISystemHomeWireToken<HomeTileValueType>
{
    public bool IsKnown => Value is "bool" or "int" or "float" or "string";
    public static HomeTileValueType Bool { get; } = new("bool");
    public static HomeTileValueType Int { get; } = new("int");
    public static HomeTileValueType Float { get; } = new("float");
    public static HomeTileValueType String { get; } = new("string");

    public static HomeTileValueType FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeTileValueType(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
