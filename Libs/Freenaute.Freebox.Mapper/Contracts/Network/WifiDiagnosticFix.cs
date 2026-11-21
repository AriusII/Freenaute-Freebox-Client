namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>
///     Wire contract documented at http://mafreebox.freebox.fr/doc/index.html#post--api-v9-wifi-diag.
///     Unset values are omitted. Explicit false and zero remain present. JSON null is not used as a patch operation.
/// </summary>
public sealed class WifiDiagnosticFix
{
	/// <summary>The operation prose explicitly allows omitting aps and/or bsss.</summary>
	[JsonPropertyName("aps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<WifiApDiagnosticFix[]>))]
	public Optional<WifiApDiagnosticFix[]> Aps { get; init; }

	/// <summary>The operation prose explicitly allows omitting aps and/or bsss.</summary>
	[JsonPropertyName("bsss")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<WifiBssDiagnosticFix[]>))]
	public Optional<WifiBssDiagnosticFix[]> Bsss { get; init; }
}
