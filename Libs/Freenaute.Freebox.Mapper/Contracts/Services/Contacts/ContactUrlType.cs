namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Exact wire values documented for ContactUrl.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactUrl.type" />
[JsonConverter(typeof(ContactUrlTypeJsonConverter))]
public readonly record struct ContactUrlType(string Value)
{
	public static ContactUrlType Profile => new(MapperConstants.ServiceWireTokens.Contacts.UrlType.Profile);
	public static ContactUrlType Blog => new(MapperConstants.ServiceWireTokens.Contacts.UrlType.Blog);
	public static ContactUrlType Site => new(MapperConstants.ServiceWireTokens.Contacts.UrlType.Site);
	public static ContactUrlType Other => new(MapperConstants.ServiceWireTokens.Contacts.UrlType.Other);

	public override string ToString() => Value ?? string.Empty;
}
