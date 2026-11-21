namespace Freenaute.Freebox.Mapper.Common.Types;

[JsonConverter(typeof(JsonStringEnumConverter<AirMediaAction>))]
public enum AirMediaAction : byte
{
	[JsonStringEnumMemberName("start")] Start,
	[JsonStringEnumMemberName("stop")] Stop
}
