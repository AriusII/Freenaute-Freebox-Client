namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Disk partition details. This API is unstable and may change.</summary>
public sealed class DiskPartition
{
	[JsonPropertyName("id")] public long? Id { get; init; }
	[JsonPropertyName("disk_id")] public long? DiskId { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("fstype")] public string? FsType { get; init; }
	[JsonPropertyName("label")] public string? Label { get; init; }
	[JsonPropertyName("path")] public EncodedFreeboxPath? Path { get; init; }
	[JsonPropertyName("total_bytes")] public long? TotalBytes { get; init; }
	[JsonPropertyName("used_bytes")] public long? UsedBytes { get; init; }
	[JsonPropertyName("free_bytes")] public long? FreeBytes { get; init; }
	[JsonPropertyName("fsck_result")] public string? FsckResult { get; init; }
	[JsonPropertyName("operation_pct")] public OperationProgress? OperationPercent { get; init; }
}
