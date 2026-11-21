namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#UPnPIGDConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class UPnPIGDConfig
{
	/// <summary>is the UPnP IGD service enabled</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>UPnP IGD protocol version Supported values are 1 / 2</summary>
	[JsonPropertyName("version")]
	public long? Version { get; init; }
}
