namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Read projection of the documented ContactUrl wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactUrl" />
public sealed record ContactUrl
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("contact_id")] public long? ContactId { get; init; }

	[JsonPropertyName("type")] public ContactUrlType? Type { get; init; }

	[JsonPropertyName("url")] public string? Url { get; init; }

	public override string ToString() => nameof(ContactUrl);
}
