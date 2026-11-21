namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Wire token from http://mafreebox.freebox.fr/doc/index.html#UpgradeState.state; unknown values are retained.</summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemUpgradeState>))]
public readonly record struct SystemUpgradeState(string Value) : ISystemHomeWireToken<SystemUpgradeState>
{
    public bool IsKnown => Value is "downloading" or "download_failed" or "checking" or "check_failed"
        or "prepare_write" or "prepare_write_failed" or "writing" or "write_failed" or "reread" or "reread_failed"
        or "commit" or "commit_failed";

    public static SystemUpgradeState Downloading { get; } = new("downloading");
    public static SystemUpgradeState DownloadFailed { get; } = new("download_failed");
    public static SystemUpgradeState Checking { get; } = new("checking");
    public static SystemUpgradeState CheckFailed { get; } = new("check_failed");
    public static SystemUpgradeState PrepareWrite { get; } = new("prepare_write");
    public static SystemUpgradeState PrepareWriteFailed { get; } = new("prepare_write_failed");
    public static SystemUpgradeState Writing { get; } = new("writing");
    public static SystemUpgradeState WriteFailed { get; } = new("write_failed");
    public static SystemUpgradeState Reread { get; } = new("reread");
    public static SystemUpgradeState RereadFailed { get; } = new("reread_failed");
    public static SystemUpgradeState Commit { get; } = new("commit");
    public static SystemUpgradeState CommitFailed { get; } = new("commit_failed");

    public static SystemUpgradeState FromWire(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new SystemUpgradeState(value);
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
