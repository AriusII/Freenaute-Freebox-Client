namespace Freenaute.Freebox.Mapper.Contracts.Pvr;

/// <summary>PVR storage medium. This API is unstable and may change.</summary>
public sealed class PvrMedia
{
	[JsonPropertyName("media")] public string? Media { get; init; }
	[JsonPropertyName("free_bytes")] public long? FreeBytes { get; init; }
	[JsonPropertyName("total_bytes")] public long? TotalBytes { get; init; }
	[JsonPropertyName("record_time")] public Dictionary<string, JsonElement>? RecordTime { get; init; }
}
