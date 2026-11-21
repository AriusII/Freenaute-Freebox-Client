namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.Language;

/// <summary>
///     Read contract from http://mafreebox.freebox.fr/doc/index.html#LanguageSupport; absent metadata remains
///     nullable.
/// </summary>
public sealed record LanguageSupport
{
	[JsonPropertyName("lang")] public string? Lang { get; init; }
	[JsonPropertyName("avalaible")] public ImmutableArray<string>? AvailableLanguages { get; init; }
}
