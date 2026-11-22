namespace Freenaute.Freebox.Mapper.Contracts.Pvr;

/// <summary>Finished or in-progress PVR recording. This API is unstable and may change.</summary>
public sealed class Frecord
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("media")] public string? Media { get; init; }
	[JsonPropertyName("path")] public string? Path { get; init; }
	[JsonPropertyName("filename")] public string? Filename { get; init; }
	[JsonPropertyName("byte_size")] public long? ByteSize { get; init; }
	[JsonPropertyName("has_record_gen")] public bool? HasRecordGen { get; init; }
	[JsonPropertyName("record_gen_id")] public long? RecordGenId { get; init; }
	[JsonPropertyName("altered")] public bool? Altered { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("error")] public string? Error { get; init; }
	[JsonPropertyName("channel_uuid")] public string? ChannelUuid { get; init; }
	[JsonPropertyName("channel_name")] public string? ChannelName { get; init; }
	[JsonPropertyName("channel_quality")] public string? ChannelQuality { get; init; }
	[JsonPropertyName("channel_type")] public string? ChannelType { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("subname")] public string? Subname { get; init; }
	[JsonPropertyName("broadcast_type")] public string? BroadcastType { get; init; }
	[JsonPropertyName("start")] public long? Start { get; init; }
	[JsonPropertyName("end")] public long? End { get; init; }
	[JsonPropertyName("secure")] public bool? Secure { get; init; }
}
