namespace Freenaute.Freebox.Mapper.Contracts.Network.Ddns;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DDNSConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class DdnsConfig
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }

	/// <summary>dns name to use to register</summary>
	[JsonPropertyName("hostname")]
	public string? Hostname { get; init; }

	/// <summary>username to use to register</summary>
	[JsonPropertyName("user")]
	public string? User { get; init; }
}
