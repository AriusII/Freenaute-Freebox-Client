namespace Freenaute.Freebox.Mapper.Contracts.Network.Xdsl;

/// <summary>xDSL line statistics. This API is unstable and may change.</summary>
public sealed class XdslStats
{
	[JsonPropertyName("maxrate")] public long? MaxRate { get; init; }
	[JsonPropertyName("rate")] public long? Rate { get; init; }
	[JsonPropertyName("snr")] public long? Snr { get; init; }
	[JsonPropertyName("attn")] public long? Attenuation { get; init; }
	[JsonPropertyName("snr_10")] public long? Snr10 { get; init; }
	[JsonPropertyName("attn_10")] public long? Attenuation10 { get; init; }
	[JsonPropertyName("fec")] public long? Fec { get; init; }
	[JsonPropertyName("crc")] public long? Crc { get; init; }
	[JsonPropertyName("hec")] public long? Hec { get; init; }
	[JsonPropertyName("es")] public long? Es { get; init; }
	[JsonPropertyName("ses")] public long? Ses { get; init; }
	[JsonPropertyName("phyr")] public bool? Phyr { get; init; }
	[JsonPropertyName("ginp")] public bool? Ginp { get; init; }
	[JsonPropertyName("nitro")] public bool? Nitro { get; init; }
	[JsonPropertyName("rxmt")] public long? Rxmt { get; init; }
	[JsonPropertyName("rxmt_corr")] public long? RxmtCorr { get; init; }
	[JsonPropertyName("rxmt_uncorr")] public long? RxmtUncorr { get; init; }
	[JsonPropertyName("rtx_tx")] public long? RtxTx { get; init; }
	[JsonPropertyName("rtx_c")] public long? RtxC { get; init; }
	[JsonPropertyName("rtx_uc")] public long? RtxUc { get; init; }
}
