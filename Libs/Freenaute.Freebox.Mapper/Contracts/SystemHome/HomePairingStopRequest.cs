namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record HomePairingStopRequest(
    [property: JsonPropertyName("session")]
    long Session) : ISystemHomeRequest
{
    [JsonPropertyName("op")] public string Operation => "stop";

    public void Validate()
    {
    }
}
