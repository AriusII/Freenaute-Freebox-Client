namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#UpgradeState; absent metadata remains nullable.</summary>
public sealed record UpgradeState
{
	[JsonPropertyName("state")] public SystemUpgradeState? State { get; init; }
	[JsonPropertyName("old_version")] public string? OldVersion { get; init; }
	[JsonPropertyName("new_version")] public string? NewVersion { get; init; }
	[JsonPropertyName("percent")] public long? Percent { get; init; }
	[JsonPropertyName("error_string")] public string? ErrorString { get; init; }
}
