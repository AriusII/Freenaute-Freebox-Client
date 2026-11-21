namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SwitchPortConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class SwitchPortConfig
{
	/// <summary>switch port id</summary>
	[JsonPropertyName("id")]
	public long? Id { get; init; }

	/// <summary>
	///     duplex Description auto auto negotiate duplex mode half force in half duplex mode full force in full duplex
	///     mode
	/// </summary>
	[JsonPropertyName("duplex")]
	public string? Duplex { get; init; }

	/// <summary>duplex Description auto auto negotiate speed 10 10Base-T 100 100Base-TX 1000 1000Base-T</summary>
	[JsonPropertyName("speed")]
	public string? Speed { get; init; }
}
