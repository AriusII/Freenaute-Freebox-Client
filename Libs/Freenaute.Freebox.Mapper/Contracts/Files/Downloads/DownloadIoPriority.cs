namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>http://mafreebox.freebox.fr/doc/index.html#Download.io_priority</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DownloadIoPriority>))]
public enum DownloadIoPriority : byte
{
	[JsonStringEnumMemberName("low")] Low,
	[JsonStringEnumMemberName("normal")] Normal,
	[JsonStringEnumMemberName("high")] High
}
