namespace Freenaute.Freebox.Mapper.Common.Types;

[JsonConverter(typeof(JsonStringEnumConverter<AirMediaMediaType>))]
public enum AirMediaMediaType : byte
{
	[JsonStringEnumMemberName("photo")] Photo,
	[JsonStringEnumMemberName("video")] Video
}
