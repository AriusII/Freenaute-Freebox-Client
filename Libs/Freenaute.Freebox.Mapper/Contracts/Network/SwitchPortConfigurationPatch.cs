namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#SwitchPortConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class SwitchPortConfigurationPatch
{
    /// <summary>
    ///     duplex Description auto auto negotiate duplex mode half force in half duplex mode full force in full duplex
    ///     mode
    /// </summary>
    [JsonPropertyName("duplex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<string>))]
    public Optional<string> Duplex { get; init; }

    /// <summary>duplex Description auto auto negotiate speed 10 10Base-T 100 100Base-TX 1000 1000Base-T</summary>
    [JsonPropertyName("speed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<string>))]
    public Optional<string> Speed { get; init; }
}
