namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented ContactAddress wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactAddress" />
public sealed record ContactAddress
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("contact_id")] public long? ContactId { get; init; }

	[JsonPropertyName("type")] public ContactAddressType? Type { get; init; }

	[JsonPropertyName("number")] public string? Number { get; init; }

	[JsonPropertyName("street")] public string? Street { get; init; }

	[JsonPropertyName("street2")] public string? Street2 { get; init; }

	[JsonPropertyName("city")] public string? City { get; init; }

	[JsonPropertyName("zipcode")] public string? Zipcode { get; init; }

	[JsonPropertyName("country")] public string? Country { get; init; }

	public override string ToString() => nameof(ContactAddress);
}
