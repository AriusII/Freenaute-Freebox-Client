namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LteTunnelDetails.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class LteTunnelDetails
{
	[JsonPropertyName("connected")] public bool? Connected { get; init; }

	[JsonPropertyName("last_error")] public string? LastError { get; init; }

	[JsonPropertyName("tx_flows_rate")] public long? TxFlowsRate { get; init; }

	[JsonPropertyName("tx_max_rate")] public long? TxMaxRate { get; init; }

	[JsonPropertyName("tx_used_rate")] public long? TxUsedRate { get; init; }

	[JsonPropertyName("rx_flows_rate")] public long? RxFlowsRate { get; init; }

	[JsonPropertyName("rx_max_rate")] public long? RxMaxRate { get; init; }

	[JsonPropertyName("rx_used_rate")] public long? RxUsedRate { get; init; }
}
