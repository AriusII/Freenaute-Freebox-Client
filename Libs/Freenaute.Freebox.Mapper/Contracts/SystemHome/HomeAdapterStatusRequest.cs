namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record HomeAdapterStatusRequest([property: JsonPropertyName("status")] HomeAdapterStatus Status)
    : ISystemHomeRequest
{
    public void Validate()
    {
        if (Status.Value is null)
        {
            throw new ArgumentException("Status must be defined.", nameof(Status));
        }
    }
}
