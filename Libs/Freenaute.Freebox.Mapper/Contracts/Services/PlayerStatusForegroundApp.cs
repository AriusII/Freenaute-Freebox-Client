namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PlayerStatusForegroundApp wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PlayerStatusForegroundApp" />
public sealed record PlayerStatusForegroundApp
{
	[JsonPropertyName("cur_url")] public string? CurUrl { get; init; }

	[JsonPropertyName("context")] public JsonElement? Context { get; init; }

	[JsonPropertyName("package")] public string? Package { get; init; }

	public override string ToString() => nameof(PlayerStatusForegroundApp);
}
