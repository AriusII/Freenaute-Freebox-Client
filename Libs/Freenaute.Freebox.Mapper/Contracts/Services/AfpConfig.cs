namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Read projection of the documented AfpConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#AfpConfig" />
public sealed record AfpConfig
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }

	[JsonPropertyName("guest_allow")] public bool? GuestAllow { get; init; }

	[JsonPropertyName("server_type")] public AfpServerType? ServerType { get; init; }

	[JsonPropertyName("login_name")] public string? LoginName { get; init; }

	public override string ToString() => nameof(AfpConfig);
}
