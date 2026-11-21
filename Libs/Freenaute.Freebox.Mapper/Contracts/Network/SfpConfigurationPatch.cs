namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SfpConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class SfpConfigurationPatch
{
    /// <summary>Indicate whether the SFP type is forced</summary>
    [JsonPropertyName("sfp_type_forced")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<bool>))]
    public Optional<bool> SfpTypeForced { get; init; }

    /// <summary>
    ///     What SFP type is forced (valid only when sfp_type_forced is true). Valid values are provided in
    ///     available_sfp_types
    /// </summary>
    [JsonPropertyName("sfp_type_forced_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<string>))]
    public Optional<string> SfpTypeForcedValue { get; init; }
}
