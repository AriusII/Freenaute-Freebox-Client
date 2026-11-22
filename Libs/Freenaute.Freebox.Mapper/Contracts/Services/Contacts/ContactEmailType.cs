namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Exact wire values documented for ContactEmail.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactEmail.type" />
[JsonConverter(typeof(ContactEmailTypeJsonConverter))]
public readonly record struct ContactEmailType(string Value)
{
	public static ContactEmailType Home => new(MapperConstants.ServiceWireTokens.Contacts.EmailType.Home);
	public static ContactEmailType Work => new(MapperConstants.ServiceWireTokens.Contacts.EmailType.Work);
	public static ContactEmailType Other => new(MapperConstants.ServiceWireTokens.Contacts.EmailType.Other);

	public override string ToString() => Value ?? string.Empty;
}
