namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PlayerStatusInformations wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PlayerStatusInformations" />
public sealed record PlayerStatusInformations
{
    [JsonPropertyName("name")] public string? Name { get; init; }

    [JsonPropertyName("last_activity")] public long? LastActivity { get; init; }

    [JsonPropertyName("capabilities")] public PlayerStatusCapabilities? Capabilities { get; init; }

    public override string ToString()
    {
        return nameof(PlayerStatusInformations);
    }
}
