namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Ledstrip;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#LedstripPlanning; absent metadata remains
///     nullable.
/// </summary>
public sealed record LedstripPlanning
{
	[JsonPropertyName("use_planning")] public bool? UsePlanning { get; init; }
	[JsonPropertyName("planning_mode")] public LedstripPlanningMode? PlanningMode { get; init; }
	[JsonPropertyName("resolution")] public long? Resolution { get; init; }
	[JsonPropertyName("mapping")] public ImmutableArray<bool>? Mapping { get; init; }
}
