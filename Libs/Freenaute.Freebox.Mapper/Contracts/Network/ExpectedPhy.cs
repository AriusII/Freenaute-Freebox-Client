namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#ExpectedPhy.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class ExpectedPhy
{
    /// <summary>state Description 2d4g 2.4GHz band 5g 5GHz band 6g 6 GHz band 60g 60GHz band</summary>
    [JsonPropertyName("band")]
    public string? Band { get; init; }

    /// <summary>id of the phy</summary>
    [JsonPropertyName("phy_id")]
    public long? PhyId { get; init; }

    /// <summary>true if the wifi card is detected</summary>
    [JsonPropertyName("detected")]
    public bool? Detected { get; init; }
}
