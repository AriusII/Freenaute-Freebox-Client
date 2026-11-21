namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>The name field is attested by the adapter examples; other type members are not defined.</summary>
public sealed record HomeAdapterType
{
    [JsonPropertyName("name")] public string? Name { get; init; }
}
