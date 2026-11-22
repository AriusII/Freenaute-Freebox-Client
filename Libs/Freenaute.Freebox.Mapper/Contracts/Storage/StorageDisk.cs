namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Storage disk details. This API is unstable and may change.</summary>
public sealed class StorageDisk
{
	[JsonPropertyName("id")] public long? Id { get; init; }
	[JsonPropertyName("type")] public string? Type { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("connector")] public long? Connector { get; init; }
	[JsonPropertyName("total_bytes")] public long? TotalBytes { get; init; }
	[JsonPropertyName("table_type")] public string? TableType { get; init; }
	[JsonPropertyName("model")] public string? Model { get; init; }
	[JsonPropertyName("serial")] public string? Serial { get; init; }
	[JsonPropertyName("firmware")] public string? Firmware { get; init; }
	[JsonPropertyName("temp")] public long? Temp { get; init; }
	[JsonPropertyName("operation_pct")] public OperationProgress? OperationPercent { get; init; }
	[JsonPropertyName("partitions")] public DiskPartition[]? Partitions { get; init; }
	[JsonPropertyName("idle")] public bool? Idle { get; init; }
	[JsonPropertyName("idle_duration")] public long? IdleDuration { get; init; }
	[JsonPropertyName("spinning")] public bool? Spinning { get; init; }
	[JsonPropertyName("active_duration")] public long? ActiveDuration { get; init; }

	[JsonPropertyName("time_before_spindown")]
	public long? TimeBeforeSpindown { get; init; }
}
