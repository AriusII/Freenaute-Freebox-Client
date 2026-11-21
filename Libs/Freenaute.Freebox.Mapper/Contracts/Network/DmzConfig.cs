namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DmzConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DmzConfig
{
	/// <summary>dmz host IP</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>is dmz enabled</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }
}
