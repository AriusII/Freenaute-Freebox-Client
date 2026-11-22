namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

/// <summary>Exact wire values documented for PlayerMediaCommand.cmd; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#post--api-v8-player-id_player-api-v6-control-mediactrl-" />
[JsonConverter(typeof(PlayerMediaCommandNameJsonConverter))]
public readonly record struct PlayerMediaCommandName(string Value)
{
	public static PlayerMediaCommandName PlayPause => new(MapperConstants.ServiceWireTokens.Players.PlayPause);
	public static PlayerMediaCommandName Stop => new(MapperConstants.ServiceWireTokens.Players.Stop);
	public static PlayerMediaCommandName Prev => new(MapperConstants.ServiceWireTokens.Players.Prev);
	public static PlayerMediaCommandName Next => new(MapperConstants.ServiceWireTokens.Players.Next);
	public static PlayerMediaCommandName SelectStream => new(MapperConstants.ServiceWireTokens.Players.SelectStream);

	public static PlayerMediaCommandName SelectAudioTrack =>
		new(MapperConstants.ServiceWireTokens.Players.SelectAudioTrack);

	public static PlayerMediaCommandName SelectSrtTrack =>
		new(MapperConstants.ServiceWireTokens.Players.SelectSrtTrack);

	public override string ToString() => Value ?? string.Empty;
}
