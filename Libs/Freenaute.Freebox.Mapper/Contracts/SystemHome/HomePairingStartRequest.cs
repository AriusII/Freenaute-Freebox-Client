namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record HomePairingStartRequest : ISystemHomeRequest
{
    [JsonPropertyName("op")] public string Operation => "start";

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; init; }

    public void Validate()
    {
    }
}
