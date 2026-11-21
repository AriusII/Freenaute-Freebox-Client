namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Profiles;

public sealed record ProfileCreated
{
	[JsonPropertyName("id")] public long? Id { get; init; }
}
