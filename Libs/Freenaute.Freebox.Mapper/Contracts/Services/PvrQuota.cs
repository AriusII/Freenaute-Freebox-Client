namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PvrQuota wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PvrQuota" />
public sealed record PvrQuota
{
    [JsonPropertyName("quota_exceeded")] public bool? QuotaExceeded { get; init; }

    [JsonPropertyName("needed_tresh")] public long? NeededTresh { get; init; }

    [JsonPropertyName("cur_tresh")] public long? CurTresh { get; init; }

    public override string ToString()
    {
        return nameof(PvrQuota);
    }
}
