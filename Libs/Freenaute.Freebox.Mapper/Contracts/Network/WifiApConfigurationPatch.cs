namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiApConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiApConfigurationPatch
{
    /// <summary>band Description 2d4g 2.4 GHz 5g 5 GHz 6g 6 GHz 60g 60 GHz</summary>
    [JsonPropertyName("band")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<string>))]
    public Optional<string> Band { get; init; }

    /// <summary>wanted channel width (in MHz) : 20 MHz 40 MHz 80 MHz 160 MHz</summary>
    [JsonPropertyName("channel_width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<StringOrInteger>))]
    public Optional<StringOrInteger> ChannelWidth { get; init; }

    /// <summary>wanted primary channel, value of 0 means automatic selection</summary>
    [JsonPropertyName("primary_channel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<long>))]
    public Optional<long> PrimaryChannel { get; init; }

    /// <summary>wanted secondary channel, value of 0 means automatic selection</summary>
    [JsonPropertyName("secondary_channel")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<long>))]
    public Optional<long> SecondaryChannel { get; init; }

    /// <summary>enable channels that require DFS</summary>
    [JsonPropertyName("dfs_enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<bool>))]
    public Optional<bool> DfsEnabled { get; init; }

    /// <summary>wifi ht config</summary>
    [JsonPropertyName("ht")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<WifiApHtConfigurationPatch>))]
    public Optional<WifiApHtConfigurationPatch> Ht { get; init; }

    /// <summary>wifi HE config</summary>
    [JsonPropertyName("he")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(OptionalJsonConverter<WifiApHeConfigurationPatch>))]
    public Optional<WifiApHeConfigurationPatch> He { get; init; }
}
