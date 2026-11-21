namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#UPnPRedir.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class UPnPRedir
{
	/// <summary>the redirection id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>is the redirection enabled</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>source IP</summary>
	[JsonPropertyName("ext_src_ip")]
	public string? ExtSrcIp { get; init; }

	/// <summary>external port</summary>
	[JsonPropertyName("ext_port")]
	public long? ExtPort { get; init; }

	/// <summary>the target IP on your LAN</summary>
	[JsonPropertyName("int_ip")]
	public string? IntIp { get; init; }

	/// <summary>the target port on your LAN</summary>
	[JsonPropertyName("int_port")]
	public long? IntPort { get; init; }

	/// <summary>the IP protocol to redirect</summary>
	[JsonPropertyName("proto")]
	public string? Proto { get; init; }

	/// <summary>a description</summary>
	[JsonPropertyName("desc")]
	public string? Desc { get; init; }

	/// <summary>seconds remaining before redirection expire</summary>
	[JsonPropertyName("remaining")]
	public long? Remaining { get; init; }

	/// <summary>lan host if available</summary>
	[JsonPropertyName("host")]
	public LanHost? Host { get; init; }
}
