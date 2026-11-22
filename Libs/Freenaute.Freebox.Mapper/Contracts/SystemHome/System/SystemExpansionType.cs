namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#SystemConfigExpansion.type; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemExpansionType>))]
public readonly record struct SystemExpansionType(string Value) : ISystemHomeWireToken<SystemExpansionType>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.SystemExpansionType.Unknown
		or MapperConstants.SystemHomeWireTokens.SystemExpansionType.DslLte
		or MapperConstants.SystemHomeWireTokens.SystemExpansionType.DslLteExternalAntennas
		or MapperConstants.SystemHomeWireTokens.SystemExpansionType.FtthP2P
		or MapperConstants.SystemHomeWireTokens.SystemExpansionType.FtthPon
		or MapperConstants.SystemHomeWireTokens.SystemExpansionType.Security;

	public static SystemExpansionType Unknown { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.Unknown);

	public static SystemExpansionType DslLte { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.DslLte);

	public static SystemExpansionType DslLteExternalAntennas { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.DslLteExternalAntennas);

	public static SystemExpansionType FtthP2P { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.FtthP2P);

	public static SystemExpansionType FtthPon { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.FtthPon);

	public static SystemExpansionType Security { get; } =
		new(MapperConstants.SystemHomeWireTokens.SystemExpansionType.Security);

	public static SystemExpansionType FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemExpansionType(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
