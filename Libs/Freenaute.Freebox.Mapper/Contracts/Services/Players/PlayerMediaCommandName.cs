namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

/// <summary>Exact wire values documented for PlayerMediaCommand.cmd; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#post--api-v8-player-id_player-api-v6-control-mediactrl-" />
[JsonConverter(typeof(PlayerMediaCommandNameJsonConverter))]
public readonly record struct PlayerMediaCommandName(string Value)
{
	public static PlayerMediaCommandName PlayPause => new("play_pause");
	public static PlayerMediaCommandName Stop => new("stop");
	public static PlayerMediaCommandName Prev => new("prev");
	public static PlayerMediaCommandName Next => new("next");
	public static PlayerMediaCommandName SelectStream => new("select_stream");
	public static PlayerMediaCommandName SelectAudioTrack => new("select_audio_track");
	public static PlayerMediaCommandName SelectSrtTrack => new("select_srt_track");

	public override string ToString() => Value ?? string.Empty;
}
