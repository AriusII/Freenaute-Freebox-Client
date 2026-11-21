namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PlayerMediaCommand wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#post--api-v8-player-id_player-api-v6-control-mediactrl-" />
public sealed record PlayerMediaCommand
{
    [JsonPropertyName("cmd")] public PlayerMediaCommandName? Cmd { get; init; }

    public override string ToString()
    {
        return nameof(PlayerMediaCommand);
    }
}
