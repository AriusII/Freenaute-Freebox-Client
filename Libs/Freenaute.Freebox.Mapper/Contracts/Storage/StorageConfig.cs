namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Storage power-management configuration. This API is unstable and may change.</summary>
public sealed class StorageConfig
{
	[JsonPropertyName("external_pm_enabled")]
	public bool? ExternalPowerManagementEnabled { get; init; }

	[JsonPropertyName("external_pm_idle_before_spindown")]
	public long? ExternalIdleBeforeSpindownMinutes { get; init; }
}
