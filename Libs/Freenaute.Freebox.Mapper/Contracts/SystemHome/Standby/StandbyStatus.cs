namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Standby;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#StandbyStatus; absent metadata remains nullable.</summary>
public sealed record StandbyStatus
{
	[JsonPropertyName("use_planning")] public bool? UsePlanning { get; init; }
	[JsonPropertyName("planning_mode")] public StandbyPlanningMode? PlanningMode { get; init; }
	[JsonPropertyName("next_change")] public long? NextChange { get; init; }

	[JsonPropertyName("available_planning_modes")]
	public ImmutableArray<StandbyPlanningMode>? AvailablePlanningModes { get; init; }
}
