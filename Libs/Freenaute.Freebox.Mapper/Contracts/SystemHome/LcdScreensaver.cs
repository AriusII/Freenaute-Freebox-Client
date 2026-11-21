namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#LcdConfig.screensaver; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<LcdScreensaver>))]
public readonly record struct LcdScreensaver(string Value) : ISystemHomeWireToken<LcdScreensaver>
{
	public bool IsKnown => Value is "disabled" or "on" or "night";
	public static LcdScreensaver Disabled { get; } = new("disabled");
	public static LcdScreensaver On { get; } = new("on");
	public static LcdScreensaver Night { get; } = new("night");

	public static LcdScreensaver FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new LcdScreensaver(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
