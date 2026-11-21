namespace Freenaute.Freebox.Mapper.Contracts.Services.ServerConfigs;

/// <summary>Exact wire values documented for AfpConfig.server_type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#AfpConfig.server_type" />
[JsonConverter(typeof(AfpServerTypeJsonConverter))]
public readonly record struct AfpServerType(string Value)
{
	public static AfpServerType Powerbook => new("powerbook");
	public static AfpServerType Powermac => new("powermac");
	public static AfpServerType Macmini => new("macmini");
	public static AfpServerType Imac => new("imac");
	public static AfpServerType Macbook => new("macbook");
	public static AfpServerType Macbookpro => new("macbookpro");
	public static AfpServerType Macbookair => new("macbookair");
	public static AfpServerType Macpro => new("macpro");
	public static AfpServerType Appletv => new("appletv");
	public static AfpServerType Airport => new("airport");
	public static AfpServerType Xserve => new("xserve");

	public override string ToString() => Value ?? string.Empty;
}
