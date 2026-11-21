namespace Freenaute.Freebox.Mapper.Contracts.Services.ServerConfigs;

/// <summary>Read projection of the documented UPnPAVConfig wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#UPnPAVConfig" />
public sealed record UPnpAvConfig
{
	[JsonPropertyName("enabled")] public bool? Enabled { get; init; }

	public override string ToString() => nameof(UPnpAvConfig);
}
