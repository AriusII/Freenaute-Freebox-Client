namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PlayerVolume wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-v8-player-id_player-api-v6-control-volume-" />
public sealed record PlayerVolume
{
	[JsonPropertyName("volume")] public int? Volume { get; init; }

	[JsonPropertyName("mute")] public bool? Mute { get; init; }

	public override string ToString() => nameof(PlayerVolume);
}
