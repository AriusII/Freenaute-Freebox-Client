namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNodeEndpointValue.value_type; unknown values
///     are retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeEndpointValueType>))]
public readonly record struct HomeEndpointValueType(string Value) : ISystemHomeWireToken<HomeEndpointValueType>
{
    public bool IsKnown => Value is "bool" or "int" or "float" or "void";
    public static HomeEndpointValueType Bool { get; } = new("bool");
    public static HomeEndpointValueType Int { get; } = new("int");
    public static HomeEndpointValueType Float { get; } = new("float");
    public static HomeEndpointValueType Void { get; } = new("void");

    public static HomeEndpointValueType FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeEndpointValueType(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
