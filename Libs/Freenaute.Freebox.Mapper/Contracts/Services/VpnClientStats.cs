namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VpnClientStats wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VpnClientStats" />
public sealed record VpnClientStats
{
    [JsonPropertyName("rate_up")] public long? RateUp { get; init; }

    [JsonPropertyName("rate_down")] public long? RateDown { get; init; }

    [JsonPropertyName("bytes_up")] public long? BytesUp { get; init; }

    [JsonPropertyName("bytes_down")] public long? BytesDown { get; init; }

    public override string ToString()
    {
        return nameof(VpnClientStats);
    }
}
