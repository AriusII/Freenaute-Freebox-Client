namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#StandbyConfig; absent metadata remains nullable.</summary>
public sealed record StandbyConfig
{
	[JsonPropertyName("use_planning")] public bool? UsePlanning { get; init; }
	[JsonPropertyName("planning_mode")] public StandbyPlanningMode? PlanningMode { get; init; }
	[JsonPropertyName("resolution")] public long? Resolution { get; init; }
	[JsonPropertyName("mapping")] public ImmutableArray<bool>? Mapping { get; init; }
}
