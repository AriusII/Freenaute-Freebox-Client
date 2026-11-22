namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Exact wire values documented for ContactNumber.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactNumber.type" />
[JsonConverter(typeof(ContactNumberTypeJsonConverter))]
public readonly record struct ContactNumberType(string Value)
{
	public static ContactNumberType Fixed => new(MapperConstants.ServiceWireTokens.Contacts.NumberType.Fixed);
	public static ContactNumberType Mobile => new(MapperConstants.ServiceWireTokens.Contacts.NumberType.Mobile);
	public static ContactNumberType Work => new(MapperConstants.ServiceWireTokens.Contacts.NumberType.Work);
	public static ContactNumberType Fax => new(MapperConstants.ServiceWireTokens.Contacts.NumberType.Fax);
	public static ContactNumberType Other => new(MapperConstants.ServiceWireTokens.Contacts.NumberType.Other);

	public override string ToString() => Value ?? string.Empty;
}
