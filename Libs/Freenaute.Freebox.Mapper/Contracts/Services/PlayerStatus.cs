namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented PlayerStatus wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PlayerStatus" />
public sealed record PlayerStatus
{
    [JsonPropertyName("power_state")] public string? PowerState { get; init; }

    [JsonPropertyName("player")] public PlayerStatusInformations? Player { get; init; }

    [JsonPropertyName("foreground_app")] public PlayerStatusForegroundApp? ForegroundApp { get; init; }

    public override string ToString()
    {
        return nameof(PlayerStatus);
    }
}
