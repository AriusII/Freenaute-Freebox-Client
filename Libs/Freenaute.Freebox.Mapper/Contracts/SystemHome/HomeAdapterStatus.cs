namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#HomeAdapter.status; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<HomeAdapterStatus>))]
public readonly record struct HomeAdapterStatus(string Value) : ISystemHomeWireToken<HomeAdapterStatus>
{
    public bool IsKnown => Value is "unplugged" or "disabled" or "active";
    public static HomeAdapterStatus Unplugged { get; } = new("unplugged");
    public static HomeAdapterStatus Disabled { get; } = new("disabled");
    public static HomeAdapterStatus Active { get; } = new("active");

    public static HomeAdapterStatus FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeAdapterStatus(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
