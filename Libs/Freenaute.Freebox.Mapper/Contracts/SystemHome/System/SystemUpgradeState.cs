namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#UpgradeState.state; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemUpgradeState>))]
public readonly record struct SystemUpgradeState(string Value) : ISystemHomeWireToken<SystemUpgradeState>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Downloading
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.DownloadFailed
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Checking
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.CheckFailed
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.PrepareWrite
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.PrepareWriteFailed
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Writing
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.WriteFailed
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Reread
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.RereadFailed
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Commit
		or MapperConstants.SystemHomeWireTokens.SystemUpgradeState.CommitFailed;

	public static SystemUpgradeState Downloading { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Downloading);

	public static SystemUpgradeState DownloadFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.DownloadFailed);

	public static SystemUpgradeState Checking { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Checking);

	public static SystemUpgradeState CheckFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.CheckFailed);

	public static SystemUpgradeState PrepareWrite { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.PrepareWrite);

	public static SystemUpgradeState PrepareWriteFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.PrepareWriteFailed);

	public static SystemUpgradeState Writing { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Writing);

	public static SystemUpgradeState WriteFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.WriteFailed);

	public static SystemUpgradeState Reread { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Reread);

	public static SystemUpgradeState RereadFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.RereadFailed);

	public static SystemUpgradeState Commit { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.Commit);

	public static SystemUpgradeState CommitFailed { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemUpgradeState.CommitFailed);

	public static SystemUpgradeState FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemUpgradeState(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
