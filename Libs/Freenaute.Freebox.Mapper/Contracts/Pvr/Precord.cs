namespace Freenaute.Freebox.Mapper.Contracts.Pvr;

/// <summary>Planned PVR recording. This API is unstable and may change.</summary>
public sealed class Precord
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("media")] public string? Media { get; init; }
	[JsonPropertyName("path")] public string? Path { get; init; }
	[JsonPropertyName("has_record_gen")] public bool? HasRecordGen { get; init; }
	[JsonPropertyName("record_gen_id")] public long? RecordGenId { get; init; }
	[JsonPropertyName("conflict")] public bool? Conflict { get; init; }
	[JsonPropertyName("overlap_list")] public long[]? OverlapList { get; init; }
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }
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
	[JsonPropertyName("legacy_uri")] public string? LegacyUri { get; init; }

	[JsonPropertyName("force_channel_name")]
	public string? ForceChannelName { get; init; }
}
