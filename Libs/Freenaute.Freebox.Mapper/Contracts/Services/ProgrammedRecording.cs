namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented Precord wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord" />
public sealed record ProgrammedRecording
{
    [JsonPropertyName("id")] public StringOrInteger? Id { get; init; }

    [JsonPropertyName("media")] public string? Media { get; init; }

    [JsonPropertyName("path")] public string? Path { get; init; }

    [JsonPropertyName("has_record_gen")] public bool? HasRecordGen { get; init; }

    [JsonPropertyName("record_gen_id")] public long? RecordGenId { get; init; }

    [JsonPropertyName("conflict")] public bool? Conflict { get; init; }

    [JsonPropertyName("overlap_list")] public long[]? OverlapList { get; init; }

    [JsonPropertyName("enabled")] public bool? Enabled { get; init; }

    [JsonPropertyName("altered")] public bool? Altered { get; init; }

    [JsonPropertyName("state")] public RecordingState? State { get; init; }

    [JsonPropertyName("error")] public RecordingError? Error { get; init; }

    [JsonPropertyName("channel_uuid")] public string? ChannelUuid { get; init; }

    [JsonPropertyName("channel_name")] public string? ChannelName { get; init; }

    [JsonPropertyName("channel_quality")] public RecordingQuality? ChannelQuality { get; init; }

    [JsonPropertyName("channel_type")] public RecordingChannelType? ChannelType { get; init; }

    [JsonPropertyName("name")] public string? Name { get; init; }

    [JsonPropertyName("subname")] public string? Subname { get; init; }

    [JsonPropertyName("broadcast_type")] public RecordingBroadcastType? BroadcastType { get; init; }

    [JsonPropertyName("start")] public long? Start { get; init; }

    [JsonPropertyName("end")] public long? End { get; init; }

    [JsonPropertyName("legacy_uri")] public string? LegacyUri { get; init; }

    [JsonPropertyName("force_channel_name")]
    public string? ForceChannelName { get; init; }

    [JsonPropertyName("margin_before")] public long? MarginBefore { get; init; }

    [JsonPropertyName("margin_after")] public long? MarginAfter { get; init; }

    public override string ToString()
    {
        return nameof(ProgrammedRecording);
    }
}
