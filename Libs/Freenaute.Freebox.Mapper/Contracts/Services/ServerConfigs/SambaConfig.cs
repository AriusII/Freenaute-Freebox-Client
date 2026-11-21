namespace Freenaute.Freebox.Mapper.Contracts.Services.ServerConfigs;

/// <summary>Read projection of the documented SambaConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#SambaConfig" />
public sealed record SambaConfig
{
	[JsonPropertyName("file_share_enabled")]
	public bool? FileShareEnabled { get; init; }

	[JsonPropertyName("print_share_enabled")]
	public bool? PrintShareEnabled { get; init; }

	[JsonPropertyName("logon_enabled")] public bool? LogonEnabled { get; init; }

	[JsonPropertyName("logon_user")] public string? LogonUser { get; init; }

	[JsonPropertyName("workgroup")] public string? Workgroup { get; init; }

	[JsonPropertyName("smbv2_enabled")] public bool? Smbv2Enabled { get; init; }

	public override string ToString() => nameof(SambaConfig);
}
