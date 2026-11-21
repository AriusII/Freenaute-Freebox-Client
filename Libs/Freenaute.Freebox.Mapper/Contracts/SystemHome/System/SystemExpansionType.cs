namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#SystemConfigExpansion.type; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemExpansionType>))]
public readonly record struct SystemExpansionType(string Value) : ISystemHomeWireToken<SystemExpansionType>
{
	public bool IsKnown => Value is "unknown" or "dsl_lte" or "dsl_lte_external_antennas" or "ftth_p2p" or "ftth_pon"
		or "security";

	public static SystemExpansionType Unknown { get; } = new("unknown");
	public static SystemExpansionType DslLte { get; } = new("dsl_lte");
	public static SystemExpansionType DslLteExternalAntennas { get; } = new("dsl_lte_external_antennas");
	public static SystemExpansionType FtthP2p { get; } = new("ftth_p2p");
	public static SystemExpansionType FtthPon { get; } = new("ftth_pon");
	public static SystemExpansionType Security { get; } = new("security");

	public static SystemExpansionType FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemExpansionType(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
