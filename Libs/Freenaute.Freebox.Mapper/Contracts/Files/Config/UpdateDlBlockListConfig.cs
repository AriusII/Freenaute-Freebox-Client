namespace Freenaute.Freebox.Mapper.Contracts.Files.Config;

/// <summary>Patch contract for download block-list configuration.</summary>
public sealed record UpdateDlBlockListConfig
{
	[JsonPropertyName("sources")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string[]>))]
	public Optional<string[]> Sources { get; init; }
}
