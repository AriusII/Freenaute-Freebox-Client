namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Lcd;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#LcdConfig.screensaver; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<LcdScreensaver>))]
public readonly record struct LcdScreensaver(string Value) : ISystemHomeWireToken<LcdScreensaver>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.LcdScreensaver.Disabled
		or MapperConstants.SystemHomeWireTokens.LcdScreensaver.On
		or MapperConstants.SystemHomeWireTokens.LcdScreensaver.Night;

	public static LcdScreensaver Disabled { get; } = new(MapperConstants.SystemHomeWireTokens.LcdScreensaver.Disabled);
	public static LcdScreensaver On { get; } = new(MapperConstants.SystemHomeWireTokens.LcdScreensaver.On);
	public static LcdScreensaver Night { get; } = new(MapperConstants.SystemHomeWireTokens.LcdScreensaver.Night);

	public static LcdScreensaver FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new LcdScreensaver(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
