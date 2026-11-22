namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Download peer blacklist entry. This API is unstable and may change.</summary>
public sealed class DownloadBlacklistEntry
{
	[JsonPropertyName("host")] public string? Host { get; init; }
	[JsonPropertyName("reason")] public string? Reason { get; init; }
	[JsonPropertyName("expire")] public long? Expire { get; init; }
	[JsonPropertyName("global")] public bool? Global { get; init; }
}
