namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Write contract from http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-task_id-trackers.</summary>
public sealed record AddDownloadTrackerRequest
{
	[JsonPropertyName("announce")] public required string Announce { get; init; }
}
