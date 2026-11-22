namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Formatting parameters for a storage disk.</summary>
public sealed class FormatStorageDiskRequest
{
	[JsonPropertyName("table_type")] public required string TableType { get; init; }
	[JsonPropertyName("fs_type")] public required string FsType { get; init; }
	[JsonPropertyName("label")] public required string Label { get; init; }
}
