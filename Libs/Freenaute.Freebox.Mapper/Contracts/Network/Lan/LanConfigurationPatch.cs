namespace Freenaute.Freebox.Mapper.Contracts.Network.Lan;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#LanConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class LanConfigurationPatch
{
	/// <summary>Freebox Server IPv4 address</summary>
	[JsonPropertyName("ip")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Ip { get; init; }

	/// <summary>Freebox Server name</summary>
	[JsonPropertyName("name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Name { get; init; }

	/// <summary>Freebox Server DNS name</summary>
	[JsonPropertyName("name_dns")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> NameDns { get; init; }

	/// <summary>Freebox Server mDNS name</summary>
	[JsonPropertyName("name_mdns")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> NameMdns { get; init; }

	/// <summary>Freebox Server netbios name</summary>
	[JsonPropertyName("name_netbios")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> NameNetbios { get; init; }
}
