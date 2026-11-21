namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record HomeEndpointValueRequest([property: JsonPropertyName("value")] HomeIoValue Value)
    : ISystemHomeRequest
{
    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(Value);
    }
}
