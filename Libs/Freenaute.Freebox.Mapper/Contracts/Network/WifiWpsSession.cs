namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiWpsSession.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class WifiWpsSession
{
    /// <summary>wps session id</summary>
    [JsonPropertyName("id")]
    public long? Id { get; init; }

    /// <summary>bss wps uuid</summary>
    [JsonPropertyName("bss_uuid")]
    public string? BssUuid { get; init; }

    /// <summary>ssid</summary>
    [JsonPropertyName("ssid")]
    public string? Ssid { get; init; }

    /// <summary>is the session active</summary>
    [JsonPropertyName("active")]
    public bool? Active { get; init; }

    /// <summary>
    ///     result of the wps session result Description success success user_canceled canceled by user self_canceled
    ///     canceled by restart of bss failed_timeout timeout while waiting for station failed_overlap another wps session was
    ///     active failed_unknown unknown failure
    /// </summary>
    [JsonPropertyName("result")]
    public string? Result { get; init; }

    /// <summary>session start date (timestamp)</summary>
    [JsonPropertyName("start_date")]
    public long? StartDate { get; init; }

    /// <summary>session end date (timestamp)</summary>
    [JsonPropertyName("end_date")]
    public StringOrInteger? EndDate { get; init; }

    /// <summary>mac of the associated client (in case of success)</summary>
    [JsonPropertyName("mac")]
    public string? Mac { get; init; }
}
