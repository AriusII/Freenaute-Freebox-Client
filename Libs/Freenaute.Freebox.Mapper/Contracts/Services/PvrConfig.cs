namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PvrConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PvrConfig" />
public sealed record PvrConfig
{
    [JsonPropertyName("margin_before")] public long? MarginBefore { get; init; }

    [JsonPropertyName("margin_after")] public long? MarginAfter { get; init; }

    public override string ToString()
    {
        return nameof(PvrConfig);
    }
}
