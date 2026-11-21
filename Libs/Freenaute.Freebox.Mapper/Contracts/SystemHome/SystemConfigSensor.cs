namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#SystemConfigSensor; absent metadata remains
///     nullable.
/// </summary>
public sealed record SystemConfigSensor
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("value")] public long? Value { get; init; }
}
