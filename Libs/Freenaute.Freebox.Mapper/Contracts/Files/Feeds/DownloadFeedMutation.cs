namespace Freenaute.Freebox.Mapper.Contracts.Files.Feeds;

/// <summary>Feed mutations use feed_id in the example, independently of id from read endpoints.</summary>
public sealed record DownloadFeedMutation
{
	[JsonPropertyName("feed_id")] public long? FeedId { get; init; }
	[JsonPropertyName("id")] public long? Id { get; init; }
	[JsonPropertyName("status")] public string? Status { get; init; }
	[JsonPropertyName("url")] public string? Url { get; init; }
	[JsonPropertyName("title")] public string? Title { get; init; }
	[JsonPropertyName("desc")] public string? Desc { get; init; }
	[JsonPropertyName("image_url")] public string? ImageUrl { get; init; }
	[JsonPropertyName("nb_read")] public long? NbRead { get; init; }
	[JsonPropertyName("nb_unread")] public long? NbUnread { get; init; }
	[JsonPropertyName("auto_download")] public bool? AutoDownload { get; init; }
	[JsonPropertyName("fetch_ts")] public long? FetchTs { get; init; }
	[JsonPropertyName("pub_ts")] public long? PubTs { get; init; }
	[JsonPropertyName("error")] public string? Error { get; init; }
}
