namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiDiagItem.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiDiagItem
{
    /// <summary>
    ///     When this item relates to an AP, this indicates the AP’s index When this item relates to a BSS, this field is
    ///     unset
    /// </summary>
    [JsonPropertyName("ap_id")]
    public long? ApId { get; init; }

    /// <summary>
    ///     When this item relates to a BSS, this field indicates the bss’s id When this item relates to an AP, this field
    ///     is unset
    /// </summary>
    [JsonPropertyName("bssid")]
    public string? Bssid { get; init; }

    /// <summary>
    ///     The code identifying which param is faulty/suboptimal Code Description all This is a the same as doing a full
    ///     reset of this AP/BSS network_disabled This changes the ‘enabled’ field in WifiBssConfig network_security This
    ///     changes the ‘encryption’ field in WifiBssConfig network_visibility This changes the ‘hide_ssid’ field in
    ///     WifiBssConfig channel_width This changes the ‘channel_width’ field in WifiApConfig channel_value This changes the
    ///     ‘channel’ &amp; ‘secondary_channel’ fields in WifiApConfig
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; init; }

    /// <summary>
    ///     Severity Description minor minor problems don’t have performance/compatibility implications major major
    ///     problems do
    /// </summary>
    [JsonPropertyName("severity")]
    public string? Severity { get; init; }
}
