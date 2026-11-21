namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record ProfileCreated
{
    [JsonPropertyName("id")] public long? Id { get; init; }
}
