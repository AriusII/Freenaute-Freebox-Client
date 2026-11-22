namespace Freenaute.Freebox.Mapper.Contracts.Files.Config;

/// <summary>Block-list configuration from the Freebox download configuration.</summary>
public sealed record DlBlockListConfig
{
	[JsonPropertyName("sources")] public string[]? Sources { get; init; }
}
