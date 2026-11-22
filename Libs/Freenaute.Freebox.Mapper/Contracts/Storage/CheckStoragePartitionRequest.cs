namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Filesystem check parameters for a storage partition.</summary>
public sealed class CheckStoragePartitionRequest
{
	[JsonPropertyName("checkmode")] public required string CheckMode { get; init; }
}
