namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKeyConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiCustomKeyConfig
{
    /// <summary>The name of the dedicated wifi network</summary>
    [JsonPropertyName("ssid")]
    public string? Ssid { get; init; }

    /// <summary>When true, the SSID name cannot be changed.</summary>
    [JsonPropertyName("ssid_read_only")]
    public bool? SsidReadOnly { get; init; }

    /// <summary>When true, the SSID used for guest network is hidden.</summary>
    [JsonPropertyName("hide_ssid")]
    public bool? HideSsid { get; init; }

    /// <summary>Encryption used for guest Wi-Fi network.</summary>
    [JsonPropertyName("encryption")]
    public string? Encryption { get; init; }
}
