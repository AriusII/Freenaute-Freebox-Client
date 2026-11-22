namespace Freenaute.Freebox.Mapper.Contracts.Services.ServerConfigs;

/// <summary>Exact wire values documented for AfpConfig.server_type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#AfpConfig.server_type" />
[JsonConverter(typeof(AfpServerTypeJsonConverter))]
public readonly record struct AfpServerType(string Value)
{
	public static AfpServerType Powerbook =>
		new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Powerbook);

	public static AfpServerType Powermac => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Powermac);
	public static AfpServerType Macmini => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Macmini);
	public static AfpServerType Imac => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Imac);
	public static AfpServerType Macbook => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Macbook);

	public static AfpServerType Macbookpro =>
		new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Macbookpro);

	public static AfpServerType Macbookair =>
		new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Macbookair);

	public static AfpServerType Macpro => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Macpro);
	public static AfpServerType Appletv => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Appletv);
	public static AfpServerType Airport => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Airport);
	public static AfpServerType Xserve => new(MapperConstants.ServiceWireTokens.ServerConfigs.AfpServerType.Xserve);

	public override string ToString() => Value ?? string.Empty;
}
