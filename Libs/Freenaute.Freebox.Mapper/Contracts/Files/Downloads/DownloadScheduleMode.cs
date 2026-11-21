namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>http://mafreebox.freebox.fr/doc/index.html#DlThrottlingConfig.schedule</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DownloadScheduleMode>))]
public enum DownloadScheduleMode : byte
{
	[JsonStringEnumMemberName("normal")] Normal,
	[JsonStringEnumMemberName("slow")] Slow,

	[JsonStringEnumMemberName("hibernate")]
	Hibernate
}
