namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

/// <summary>Read projection of the documented PlayerOpenRequest wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#post--api-v8-player-id_player-api-v6-control-open" />
public sealed record PlayerOpenRequest
{
	[JsonPropertyName("url")] public string? Url { get; init; }

	[JsonPropertyName("type")] public string? Type { get; init; }

	public override string ToString() => nameof(PlayerOpenRequest);
}
