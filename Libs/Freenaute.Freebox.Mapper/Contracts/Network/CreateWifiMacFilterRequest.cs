namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiMacFilter.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class CreateWifiMacFilterRequest
{
	/// <summary>MAC address to filter</summary>
	[JsonPropertyName("mac")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Mac { get; init; }

	/// <summary>comment</summary>
	[JsonPropertyName("comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Comment { get; init; }

	/// <summary>
	///     type Description whitelist if mac_filter is set to whitelist this station will be allowed blacklist if
	///     mac_filter is set to blacklist this station will be rejected
	/// </summary>
	[JsonPropertyName("type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Type { get; init; }
}
