namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Language;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#LanguageSupport; absent metadata remains
///     nullable.
/// </summary>
public sealed record LanguageSupport
{
	[JsonPropertyName("lang")] public string? Lang { get; init; }

	/// <summary>Wire name is the server-side typo "avalaible" (sic) — kept verbatim on purpose; do not "fix".</summary>
	[JsonPropertyName("avalaible")]
	public ImmutableArray<string>? AvailableLanguages { get; init; }
}
