namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record RenameHomeNodeRequest([property: JsonPropertyName("label")] string Label) : ISystemHomeRequest
{
    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(Label);
    }
}
