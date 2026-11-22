namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Write contract from http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-blacklist.</summary>
public sealed record AddDownloadBlacklistEntryRequest
{
	[JsonPropertyName("host")] public required string Host { get; init; }
	[JsonPropertyName("expire")] public required long Expire { get; init; }
}
