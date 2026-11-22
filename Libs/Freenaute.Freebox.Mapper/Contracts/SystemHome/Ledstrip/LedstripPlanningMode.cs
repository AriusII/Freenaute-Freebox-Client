namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Ledstrip;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#LedstripPlanning.planning_mode; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<LedstripPlanningMode>))]
public readonly record struct LedstripPlanningMode(string Value) : ISystemHomeWireToken<LedstripPlanningMode>
{
	public bool IsKnown => Value is MapperConstants.SystemHomeWireTokens.LedstripPlanningMode.LedstripOff;

	public static LedstripPlanningMode LedstripOff { get; } =
		new(MapperConstants.SystemHomeWireTokens.LedstripPlanningMode.LedstripOff);

	public static LedstripPlanningMode FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new LedstripPlanningMode(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
