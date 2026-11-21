namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#UpdateStatus.state; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemUpdateState>))]
public readonly record struct SystemUpdateState(string Value) : ISystemHomeWireToken<SystemUpdateState>
{
	public bool IsKnown => Value is "initializing" or "upgrading" or "up_to_date" or "error";
	public static SystemUpdateState Initializing { get; } = new("initializing");
	public static SystemUpdateState Upgrading { get; } = new("upgrading");
	public static SystemUpdateState UpToDate { get; } = new("up_to_date");
	public static SystemUpdateState Error { get; } = new("error");

	public static SystemUpdateState FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemUpdateState(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
