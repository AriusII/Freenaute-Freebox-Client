namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LteTunnel.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LteTunnel
{
	[JsonPropertyName("lte")] public LteTunnelDetails? Lte { get; init; }

	[JsonPropertyName("xdsl")] public LteTunnelDetails? Xdsl { get; init; }
}
