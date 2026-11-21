namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented VoicemailEntry wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VoicemailEntry" />
public sealed record VoicemailEntry
{
	[JsonPropertyName("id")] public string? Id { get; init; }

	[JsonPropertyName("country_code")] public StringOrInteger? CountryCode { get; init; }

	[JsonPropertyName("phone_number")] public string? PhoneNumber { get; init; }

	[JsonPropertyName("date")] public long? Date { get; init; }

	[JsonPropertyName("read")] public bool? Read { get; init; }

	[JsonPropertyName("duration")] public long? Duration { get; init; }

	public override string ToString() => nameof(VoicemailEntry);
}
