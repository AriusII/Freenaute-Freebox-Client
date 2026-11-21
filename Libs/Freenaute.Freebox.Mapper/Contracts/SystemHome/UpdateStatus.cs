namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#UpdateStatus; absent metadata remains nullable.</summary>
public sealed record UpdateStatus
{
    [JsonPropertyName("state")] public SystemUpdateState? State { get; init; }
    [JsonPropertyName("upgrade_state")] public UpgradeState? UpgradeState { get; init; }
}
