namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-diag.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiApDiagnosticFix
{
	[JsonPropertyName("ap_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> ApId { get; init; }

	[JsonPropertyName("code")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Code { get; init; }
}
