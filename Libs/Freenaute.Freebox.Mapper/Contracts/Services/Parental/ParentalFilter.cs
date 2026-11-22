namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>Read projection of the documented ParentalFilter wire object.</summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilter" />
public sealed record ParentalFilter
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("macs")] public string[]? Macs { get; init; }

	[JsonPropertyName("hosts")] public string[]? Hosts { get; init; }

	/// <summary>Deprecated IP field retained only for compatibility with old rules.</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	[JsonPropertyName("desc")] public string? Desc { get; init; }

	[JsonPropertyName("forced")] public bool? Forced { get; init; }

	[JsonPropertyName("forced_mode")] public ParentalFilterState? ForcedMode { get; init; }

	[JsonPropertyName("tmp_mode_expire")] public long? TmpModeExpire { get; init; }

	[JsonPropertyName("tmp_mode")] public ParentalFilterState? TmpMode { get; init; }

	[JsonPropertyName("scheduling_mode")] public ParentalSchedulingMode? SchedulingMode { get; init; }

	[JsonPropertyName("filter_state")] public ParentalFilterState? FilterState { get; init; }

	[JsonPropertyName("current_mapping_idx")]
	public long? CurrentMappingIdx { get; init; }

	[JsonPropertyName("next_change")] public long? NextChange { get; init; }

	public override string ToString() => nameof(ParentalFilter);
}
