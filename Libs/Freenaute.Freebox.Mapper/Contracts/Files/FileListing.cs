namespace Freenaute.Freebox.Mapper.Contracts.Files;

public sealed record FileListing
{
    [JsonPropertyName("entries")] public FileEntry[]? Entries { get; init; }
    [JsonPropertyName("cursor")] public string? Cursor { get; init; }
}
