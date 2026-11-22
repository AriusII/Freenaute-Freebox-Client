namespace Freenaute.Freebox.Mapper.Contracts.Files.Downloads;

/// <summary>Torrent download peer. This API is unstable and may change.</summary>
public sealed class DownloadPeer
{
	[JsonPropertyName("host")] public string? Host { get; init; }
	[JsonPropertyName("port")] public long? Port { get; init; }
	[JsonPropertyName("state")] public string? State { get; init; }
	[JsonPropertyName("origin")] public string? Origin { get; init; }
	[JsonPropertyName("protocol")] public string? Protocol { get; init; }
	[JsonPropertyName("client")] public string? Client { get; init; }
	[JsonPropertyName("country_code")] public string? CountryCode { get; init; }
	[JsonPropertyName("tx")] public long? Tx { get; init; }
	[JsonPropertyName("rx")] public long? Rx { get; init; }
	[JsonPropertyName("tx_rate")] public long? TxRate { get; init; }
	[JsonPropertyName("rx_rate")] public long? RxRate { get; init; }
	[JsonPropertyName("progress")] public long? Progress { get; init; }
	[JsonPropertyName("requests")] public long[]? Requests { get; init; }
}
