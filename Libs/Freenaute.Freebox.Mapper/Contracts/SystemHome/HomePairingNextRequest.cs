namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record HomePairingNextRequest : ISystemHomeRequest
{
    [JsonPropertyName("op")] public string Operation => "next";
    [JsonPropertyName("session")] public StringOrInteger Session { get; init; }
    [JsonPropertyName("pageid")] public StringOrInteger PageId { get; init; }
    [JsonPropertyName("fields")] public ImmutableArray<HomeIoValue> Fields { get; init; } = [];

    public void Validate()
    {
        if (Session.Kind == default || PageId.Kind == default)
        {
            throw new ArgumentException("Pairing identifiers must be defined.");
        }

        if (Fields.IsDefault || Fields.Any(static value => value is null || value.Kind == HomeIoValueKind.Float))
        {
            throw new ArgumentException("Pairing fields require null, Boolean, integer or string values.",
                nameof(Fields));
        }
    }
}
