namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.NetworkControls;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#net-object; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<NetworkControlMode>))]
public readonly record struct NetworkControlMode(string Value) : ISystemHomeWireToken<NetworkControlMode>
{
	public bool IsKnown => Value is "allowed" or "denied" or "webonly";
	public static NetworkControlMode Allowed { get; } = new("allowed");
	public static NetworkControlMode Denied { get; } = new("denied");
	public static NetworkControlMode Webonly { get; } = new("webonly");

	public static NetworkControlMode FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new NetworkControlMode(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
