namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Torrent download tracker. This API is unstable and may change.</summary>
public sealed class DownloadTracker
{
	[JsonPropertyName("announce")] public string? Announce { get; init; }
	[JsonPropertyName("is_backup")] public bool? IsBackup { get; init; }
	[JsonPropertyName("status")] public string? Status { get; init; }
	[JsonPropertyName("interval")] public long? Interval { get; init; }
	[JsonPropertyName("min_interval")] public long? MinInterval { get; init; }
	[JsonPropertyName("reannounce_in")] public long? ReannounceIn { get; init; }
	[JsonPropertyName("nseeders")] public long? SeederCount { get; init; }
	[JsonPropertyName("nleechers")] public long? LeecherCount { get; init; }
	[JsonPropertyName("is_enabled")] public bool? IsEnabled { get; init; }
}
