namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented Media wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Media" />
public sealed record PvrMedia
{
    [JsonPropertyName("media")] public string? Media { get; init; }

    [JsonPropertyName("free_bytes")] public long? FreeBytes { get; init; }

    [JsonPropertyName("total_bytes")] public long? TotalBytes { get; init; }

    [JsonPropertyName("record_time")] public PvrRecordTime? RecordTime { get; init; }

    public override string ToString()
    {
        return nameof(PvrMedia);
    }
}
