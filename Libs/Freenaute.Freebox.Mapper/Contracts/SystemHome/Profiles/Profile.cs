namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Profiles;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#Profile; absent metadata remains nullable.</summary>
public sealed record Profile
{
	[JsonPropertyName("id")] public long? Id { get; init; }
	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("icon")] public string? Icon { get; init; }

	// Request/response examples use url while the declaration says icon.
	[JsonPropertyName("url")] public string? Url { get; init; }
}
