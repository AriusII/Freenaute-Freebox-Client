namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeNode.status; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeNodeStatus>))]
public readonly record struct HomeNodeStatus(string Value) : ISystemHomeWireToken<HomeNodeStatus>
{
    public bool IsKnown => Value is "unreachable" or "disabled" or "active" or "unpaired";
    public static HomeNodeStatus Unreachable { get; } = new("unreachable");
    public static HomeNodeStatus Disabled { get; } = new("disabled");
    public static HomeNodeStatus Active { get; } = new("active");
    public static HomeNodeStatus Unpaired { get; } = new("unpaired");

    public static HomeNodeStatus FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeNodeStatus(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
