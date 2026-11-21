namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

public sealed record DownloadThrottlingStatus
{
	[JsonPropertyName("is_scheduled")] public bool? IsScheduled { get; init; }
	[JsonPropertyName("throttling")] public string? Throttling { get; init; }
}
