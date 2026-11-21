namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#XdslStats.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class XdslStats
{
    /// <summary>ATM max rate in kbit/s</summary>
    [JsonPropertyName("maxrate")]
    public long? Maxrate { get; init; }

    /// <summary>ATM rate in kbit/s</summary>
    [JsonPropertyName("rate")]
    public long? Rate { get; init; }

    /// <summary>in dB</summary>
    [JsonPropertyName("snr")]
    public long? Snr { get; init; }

    /// <summary>in dB</summary>
    [JsonPropertyName("attn")]
    public long? Attn { get; init; }

    /// <summary>in dB/10</summary>
    [JsonPropertyName("snr_10")]
    public long? Snr10 { get; init; }

    /// <summary>in dB/10</summary>
    [JsonPropertyName("attn_10")]
    public long? Attn10 { get; init; }

    [JsonPropertyName("fec")] public long? Fec { get; init; }

    [JsonPropertyName("crc")] public long? Crc { get; init; }

    [JsonPropertyName("hec")] public long? Hec { get; init; }

    [JsonPropertyName("es")] public long? Es { get; init; }

    [JsonPropertyName("ses")] public long? Ses { get; init; }

    [JsonPropertyName("phyr")] public bool? Phyr { get; init; }

    [JsonPropertyName("ginp")] public bool? Ginp { get; init; }

    [JsonPropertyName("nitro")] public bool? Nitro { get; init; }

    /// <summary>only available when phyr is on</summary>
    [JsonPropertyName("rxmt")]
    public long? Rxmt { get; init; }

    /// <summary>only available when phyr is on</summary>
    [JsonPropertyName("rxmt_corr")]
    public long? RxmtCorr { get; init; }

    /// <summary>only available when phyr is on</summary>
    [JsonPropertyName("rxmt_uncorr")]
    public long? RxmtUncorr { get; init; }

    /// <summary>only available when ginp is on</summary>
    [JsonPropertyName("rtx_tx")]
    public long? RtxTx { get; init; }

    /// <summary>only available when ginp is on</summary>
    [JsonPropertyName("rtx_c")]
    public long? RtxC { get; init; }

    /// <summary>only available when ginp is on</summary>
    [JsonPropertyName("rtx_uc")]
    public long? RtxUc { get; init; }
}
