namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed record DefaultModeMigrationStatus
{
	[JsonPropertyName("default_mode_migrated")]
	public bool? DefaultModeMigrated { get; init; }
}
