namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Set the state of a storage partition.</summary>
public sealed class SetStoragePartitionStateRequest
{
	[JsonPropertyName("state")] public required string State { get; init; }
}
