namespace Freenaute.Freebox.Mapper.Contracts.Storage;

/// <summary>Set the state of a storage disk.</summary>
public sealed class SetStorageDiskStateRequest
{
	[JsonPropertyName("state")] public required string State { get; init; }
}
