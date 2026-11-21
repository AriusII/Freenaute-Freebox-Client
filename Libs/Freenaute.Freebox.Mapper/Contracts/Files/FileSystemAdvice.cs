namespace Freenaute.Freebox.Mapper.Contracts.Files;

public sealed record FileSystemAdvice
{
    [JsonPropertyName("fstype")] public string? FsType { get; init; }
    [JsonPropertyName("table_type")] public string? TableType { get; init; }
    [JsonPropertyName("reason")] public string? Reason { get; init; }

    [JsonPropertyName("partitions_to_delete")]
    public DiskPartition[]? PartitionsToDelete { get; init; }
}
