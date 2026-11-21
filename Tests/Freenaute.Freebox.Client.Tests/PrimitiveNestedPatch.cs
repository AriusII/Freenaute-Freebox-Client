namespace Freenaute.Freebox.Client.Tests;

public sealed record PrimitiveNestedPatch
{
	[JsonPropertyName("item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<PrimitiveItem>))]
	public Optional<PrimitiveItem> Item { get; init; }

	[JsonPropertyName("values")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<int[]>))]
	public Optional<int[]> Values { get; init; }
}
