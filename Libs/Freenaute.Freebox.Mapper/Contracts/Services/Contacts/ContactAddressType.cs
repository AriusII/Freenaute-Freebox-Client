namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Exact wire values documented for ContactAddress.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactAddress.type" />
[JsonConverter(typeof(ContactAddressTypeJsonConverter))]
public readonly record struct ContactAddressType(string Value)
{
	public static ContactAddressType Home => new("home");
	public static ContactAddressType Work => new("work");
	public static ContactAddressType Other => new("other");

	public override string ToString() => Value ?? string.Empty;
}
