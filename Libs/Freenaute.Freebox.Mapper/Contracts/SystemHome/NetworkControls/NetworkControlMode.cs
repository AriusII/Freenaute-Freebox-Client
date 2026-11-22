namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.NetworkControls;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#net-object; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<NetworkControlMode>))]
public readonly record struct NetworkControlMode(string Value) : ISystemHomeWireToken<NetworkControlMode>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.NetworkControlMode.Allowed
		or MapperConstants.SystemHomeWireTokens.NetworkControlMode.Denied
		or MapperConstants.SystemHomeWireTokens.NetworkControlMode.Webonly;

	public static NetworkControlMode Allowed { get; } =
		new(MapperConstants.SystemHomeWireTokens.NetworkControlMode.Allowed);

	public static NetworkControlMode Denied { get; } =
		new(MapperConstants.SystemHomeWireTokens.NetworkControlMode.Denied);

	public static NetworkControlMode Webonly { get; } =
		new(MapperConstants.SystemHomeWireTokens.NetworkControlMode.Webonly);

	public static NetworkControlMode FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new NetworkControlMode(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
