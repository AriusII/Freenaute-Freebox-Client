namespace Freenaute.Freebox.Mapper.Contracts.Files;

public sealed record RrdResult
{
    [JsonPropertyName("date_start")] public long? DateStart { get; init; }
    [JsonPropertyName("date_end")] public long? DateEnd { get; init; }
    [JsonPropertyName("data")] public Dictionary<string, long?>[]? Data { get; init; }
}
