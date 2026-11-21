namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#FreeplugNetwork.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class FreeplugNetwork
{
	/// <summary>Network unique id</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>List of freeplugs member of this network</summary>
	[JsonPropertyName("members")]
	public Freeplug[]? Members { get; init; }
}
