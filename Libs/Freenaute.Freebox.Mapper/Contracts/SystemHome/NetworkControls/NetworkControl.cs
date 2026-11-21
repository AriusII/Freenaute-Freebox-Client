namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.NetworkControls;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#NetworkControl; absent metadata remains
///     nullable.
/// </summary>
public sealed record NetworkControl
{
	[JsonPropertyName("profile_id")] public long? ProfileId { get; init; }
	[JsonPropertyName("next_change")] public long? NextChange { get; init; }
	[JsonPropertyName("override_mode")] public NetworkControlMode? OverrideMode { get; init; }
	[JsonPropertyName("current_mode")] public NetworkControlMode? CurrentMode { get; init; }
	[JsonPropertyName("rule_mode")] public NetworkControlMode? RuleMode { get; init; }
	[JsonPropertyName("override_until")] public long? OverrideUntil { get; init; }
	[JsonPropertyName("override")] public bool? Override { get; init; }
	[JsonPropertyName("macs")] public ImmutableArray<string>? Macs { get; init; }
	[JsonPropertyName("hosts")] public NetworkControlHosts? Hosts { get; init; }
	[JsonPropertyName("resolution")] public long? Resolution { get; init; }
	[JsonPropertyName("cdayranges")] public ImmutableArray<string>? CustomDayRanges { get; init; }
}
