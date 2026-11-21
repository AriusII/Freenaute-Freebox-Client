namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#SystemConfigFan; absent metadata remains
///     nullable.
/// </summary>
public sealed record SystemConfigFan
{
	[JsonPropertyName("id")] public string? Id { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }
	[JsonPropertyName("value")] public long? Value { get; init; }
}
