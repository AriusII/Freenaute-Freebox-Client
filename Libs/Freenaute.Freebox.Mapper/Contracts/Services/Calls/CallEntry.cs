namespace Freenaute.Freebox.Mapper.Contracts.Services.Calls;

/// <summary>Read projection of the documented CallEntry wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#CallEntry" />
public sealed record CallEntry
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("type")] public CallType? Type { get; init; }

	[JsonPropertyName("datetime")] public long? Datetime { get; init; }

	[JsonPropertyName("number")] public string? Number { get; init; }

	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("duration")] public long? Duration { get; init; }

	[JsonPropertyName("new")] public bool? New { get; init; }

	[JsonPropertyName("contact_id")] public long? ContactId { get; init; }

	[JsonPropertyName("line_id")] public long? LineId { get; init; }

	public override string ToString() => nameof(CallEntry);
}
