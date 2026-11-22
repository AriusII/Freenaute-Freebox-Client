namespace Freenaute.Freebox.Mapper.Contracts.Network.VpnClient;

/// <summary>VPN client traffic statistics. This API is unstable and may change.</summary>
public sealed class VpnClientStats
{
	[JsonPropertyName("rate_up")] public long? RateUp { get; init; }
	[JsonPropertyName("rate_down")] public long? RateDown { get; init; }
	[JsonPropertyName("bytes_up")] public long? BytesUp { get; init; }
	[JsonPropertyName("bytes_down")] public long? BytesDown { get; init; }
}
