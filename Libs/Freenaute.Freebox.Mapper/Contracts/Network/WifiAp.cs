namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiAp.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiAp
{
	/// <summary>wifi ap id</summary>
	[JsonPropertyName("id")]
	public long? Id { get; init; }

	/// <summary>wifi ap name</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>ap status</summary>
	[JsonPropertyName("status")]
	public WifiApStatus? Status { get; init; }

	/// <summary>ap capabilities</summary>
	[JsonPropertyName("capabilities")]
	public WifiApCapabilities? Capabilities { get; init; }

	/// <summary>ap configuration</summary>
	[JsonPropertyName("config")]
	public WifiApConfig? Config { get; init; }
}
