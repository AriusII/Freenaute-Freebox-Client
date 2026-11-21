namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanHostName.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LanHostName
{
	/// <summary>Host name</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>source of the name</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }
}
