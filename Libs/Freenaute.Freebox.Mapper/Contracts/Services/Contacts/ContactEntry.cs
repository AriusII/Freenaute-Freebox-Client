namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Read projection of the documented ContactEntry wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactEntry" />
public sealed record ContactEntry
{
	[JsonPropertyName("id")] public long? Id { get; init; }

	[JsonPropertyName("display_name")] public string? DisplayName { get; init; }

	[JsonPropertyName("first_name")] public string? FirstName { get; init; }

	[JsonPropertyName("last_name")] public string? LastName { get; init; }

	[JsonPropertyName("company")] public string? Company { get; init; }

	[JsonPropertyName("photo_url")] public string? PhotoUrl { get; init; }

	[JsonPropertyName("last_update")] public long? LastUpdate { get; init; }

	[JsonPropertyName("notes")] public string? Notes { get; init; }

	[JsonPropertyName("addresses")] public ContactAddress[]? Addresses { get; init; }

	[JsonPropertyName("emails")] public ContactEmail[]? Emails { get; init; }

	[JsonPropertyName("numbers")] public ContactNumber[]? Numbers { get; init; }

	[JsonPropertyName("urls")] public ContactUrl[]? Urls { get; init; }

	[JsonPropertyName("birthday")] public string? Birthday { get; init; }

	public override string ToString() => nameof(ContactEntry);
}
