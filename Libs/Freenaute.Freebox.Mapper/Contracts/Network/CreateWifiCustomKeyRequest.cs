namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#WifiCustomKeyParams.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class CreateWifiCustomKeyRequest
{
	/// <summary>description of the custom key</summary>
	[JsonPropertyName("description")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Description { get; init; }

	/// <summary>Wi-Fi password for this custom access “ **** ” will be returned when insufficient permission</summary>
	[JsonPropertyName("key")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Key { get; init; }

	/// <summary>
	///     Number of different hosts that can connect to this network (maximum 127) 0 has special meaning, it means
	///     unlimited number of users.
	/// </summary>
	[JsonPropertyName("max_use_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<StringOrInteger>))]
	public Optional<StringOrInteger> MaxUseCount { get; init; }

	/// <summary>Number of seconds before the custom access is revoked</summary>
	[JsonPropertyName("duration")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> Duration { get; init; }

	/// <summary>
	///     access_type Description full stations will get full access to local network + internet net_only stations
	///     connected using this custom key will be isolated and won’t have access to local network devices
	/// </summary>
	[JsonPropertyName("access_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> AccessType { get; init; }
}
