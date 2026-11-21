namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>http://mafreebox.freebox.fr/doc/index.html#DlThrottlingConfig.mode</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DownloadThrottlingMode>))]
public enum DownloadThrottlingMode : byte
{
	[JsonStringEnumMemberName("normal")] Normal,
	[JsonStringEnumMemberName("slow")] Slow,

	[JsonStringEnumMemberName("hibernate")]
	Hibernate,
	[JsonStringEnumMemberName("schedule")] Schedule
}
