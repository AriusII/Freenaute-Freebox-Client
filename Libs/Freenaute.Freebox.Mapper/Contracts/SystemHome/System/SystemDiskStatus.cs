namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#SystemConfig.disk_status; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemDiskStatus>))]
public readonly record struct SystemDiskStatus(string Value) : ISystemHomeWireToken<SystemDiskStatus>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.SystemDiskStatus.NotDetected
		or MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Disabled
		or MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Initializing
		or MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Error
		or MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Active;

	public static SystemDiskStatus NotDetected { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemDiskStatus.NotDetected);

	public static SystemDiskStatus Disabled { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Disabled);

	public static SystemDiskStatus Initializing { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Initializing);

	public static SystemDiskStatus Error { get; } = new(MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Error);
	public static SystemDiskStatus Active { get; } = new(MapperConstants.SystemHomeWireTokens.SystemDiskStatus.Active);

	public static SystemDiskStatus FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemDiskStatus(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
