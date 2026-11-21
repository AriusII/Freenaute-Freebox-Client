namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SfpConfig.
///     Missing read values are represented by nullable properties; this does not assert server acceptance of JSON null.
/// </summary>
public sealed class SfpConfig
{
    /// <summary>Indicate whether the SFP type is forced</summary>
    [JsonPropertyName("sfp_type_forced")]
    public bool? SfpTypeForced { get; init; }

    /// <summary>
    ///     What SFP type is forced (valid only when sfp_type_forced is true). Valid values are provided in
    ///     available_sfp_types
    /// </summary>
    [JsonPropertyName("sfp_type_forced_value")]
    public string? SfpTypeForcedValue { get; init; }

    /// <summary>
    ///     array containing what SFP types can be configured on the LAN SFP port. Possible values are listed in the
    ///     following table: Type Description p2p_1g 1000BASE-X p2p_2d5g_no_aneg 2500BASE-X p2p_10g 10GBASE-R copper_1g
    ///     1000BASE-T copper_sgmii_1g SGMII copper_sgmii_10g USXGMII
    /// </summary>
    [JsonPropertyName("available_sfp_types")]
    public string[]? AvailableSfpTypes { get; init; }
}
