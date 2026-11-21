namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#DDNSConfig.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class DdnsConfigurationPatch
{
	[JsonPropertyName("enabled")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Enabled { get; init; }

	/// <summary>dns name to use to register</summary>
	[JsonPropertyName("hostname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Hostname { get; init; }

	/// <summary>password to use to register</summary>
	[JsonPropertyName("password")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Password { get; init; }

	/// <summary>username to use to register</summary>
	[JsonPropertyName("user")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> User { get; init; }
}
