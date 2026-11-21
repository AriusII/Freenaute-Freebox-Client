namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented Frecord wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Frecord" />
public sealed record FinishedRecording
{
    [JsonPropertyName("id")] public StringOrInteger? Id { get; init; }

    [JsonPropertyName("media")] public string? Media { get; init; }

    [JsonPropertyName("path")] public string? Path { get; init; }

    [JsonPropertyName("filename")] public string? Filename { get; init; }

    [JsonPropertyName("byte_size")] public long? ByteSize { get; init; }

    [JsonPropertyName("has_record_gen")] public bool? HasRecordGen { get; init; }

    [JsonPropertyName("record_gen_id")] public long? RecordGenId { get; init; }

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

    [JsonPropertyName("secure")] public bool? Secure { get; init; }

    [JsonPropertyName("enabled")] public bool? Enabled { get; init; }

    public override string ToString()
    {
        return nameof(FinishedRecording);
    }
}
