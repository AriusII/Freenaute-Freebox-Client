namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiBssConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiBssConfig
{
    /// <summary>
    ///     enable this BSS. Note that if you want the AP to completely stop emitting wifi you should use WifiGlobalConfig
    ///     enabled attribute.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; init; }

    /// <summary>bss displayed name</summary>
    [JsonPropertyName("ssid")]
    public string? Ssid { get; init; }

    /// <summary>don’t show bss in bss list</summary>
    [JsonPropertyName("hide_ssid")]
    public StringOrBoolean? HideSsid { get; init; }

    /// <summary>Whether or not to use GCMP-256 (only in WPA3 &amp; for box that supports 802.11-be)</summary>
    [JsonPropertyName("gcmp256")]
    public string? Gcmp256 { get; init; }

    /// <summary>
    ///     encryption Description wep wep (should not use) wpa_psk_auto wpa1 CCMP+TKIP (should not use) wpa_psk_tkip wpa1
    ///     TKIP (should not use) wpa_psk_ccmp wpa1 CCMP (should not use) wpa12_psk_auto wpa1+wpa2 CCMP+TKIP (should not use)
    ///     wpa2_psk_auto wpa2 CCMP+TKIP (should not use) wpa2_psk_tkip wpa2 TKIP (should not use) wpa2_psk_ccmp wpa2 CCMP
    ///     wpa23_psk_ccmp wpa2+wpa3 CCMP WPA3-personal transition mode wpa23_psk_ccmp_mrsno wpa2+wpa3 CCMP WPA3-personal
    ///     compatibility mode wpa3_psk_ccmp wpa3 CCMP WPA3-personal only mode
    /// </summary>
    [JsonPropertyName("encryption")]
    public string? Encryption { get; init; }

    /// <summary>wifi key “ **** ” will be returned when insufficient permission</summary>
    [JsonPropertyName("key")]
    public string? Key { get; init; }

    /// <summary>eapol version</summary>
    [JsonPropertyName("eapol_version")]
    public long? EapolVersion { get; init; }

    [JsonPropertyName("wps_enabled")] public bool? WpsEnabled { get; init; }

    [JsonPropertyName("wps_uuid")] public string? WpsUuid { get; init; }
}
