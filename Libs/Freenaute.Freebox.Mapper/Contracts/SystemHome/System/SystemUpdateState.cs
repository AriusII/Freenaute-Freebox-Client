namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#UpdateStatus.state; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemUpdateState>))]
public readonly record struct SystemUpdateState(string Value) : ISystemHomeWireToken<SystemUpdateState>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.SystemUpdateState.Initializing
		or MapperConstants.SystemHomeWireTokens.SystemUpdateState.Upgrading
		or MapperConstants.SystemHomeWireTokens.SystemUpdateState.UpToDate
		or MapperConstants.SystemHomeWireTokens.SystemUpdateState.Error;

	public static SystemUpdateState Initializing { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpdateState.Initializing);

	public static SystemUpdateState Upgrading { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpdateState.Upgrading);

	public static SystemUpdateState UpToDate { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpdateState.UpToDate);

	public static SystemUpdateState Error { get; } = new(MapperConstants.SystemHomeWireTokens.SystemUpdateState.Error);

	public static SystemUpdateState FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemUpdateState(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
