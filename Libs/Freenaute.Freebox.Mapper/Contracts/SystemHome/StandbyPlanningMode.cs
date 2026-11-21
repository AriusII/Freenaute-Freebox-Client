namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#StandbyConfig.planning_mode; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<StandbyPlanningMode>))]
public readonly record struct StandbyPlanningMode(string Value) : ISystemHomeWireToken<StandbyPlanningMode>
{
	public bool IsKnown => Value is "wifi_off" or "standby";
	public static StandbyPlanningMode WifiOff { get; } = new("wifi_off");
	public static StandbyPlanningMode Standby { get; } = new("standby");

	public static StandbyPlanningMode FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new StandbyPlanningMode(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
