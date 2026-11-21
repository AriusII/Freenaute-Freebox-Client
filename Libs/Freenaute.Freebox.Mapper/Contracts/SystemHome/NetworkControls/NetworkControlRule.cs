namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.NetworkControls;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#NetworkControlRule; absent metadata remains
///     nullable.
/// </summary>
public sealed record NetworkControlRule
{
	[JsonPropertyName("id")] public long? Id { get; init; }
	[JsonPropertyName("profile_id")] public long? ProfileId { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("mode")] public string? Mode { get; init; }
	[JsonPropertyName("start_time")] public long? StartTime { get; init; }
	[JsonPropertyName("end_time")] public long? EndTime { get; init; }
	[JsonPropertyName("weekdays")] public ImmutableArray<bool>? Weekdays { get; init; }
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }
}
