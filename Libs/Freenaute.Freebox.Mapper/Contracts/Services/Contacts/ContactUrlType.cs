namespace Freenaute.Freebox.Mapper.Contracts.Services.Contacts;

/// <summary>Exact wire values documented for ContactUrl.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactUrl.type" />
[JsonConverter(typeof(ContactUrlTypeJsonConverter))]
public readonly record struct ContactUrlType(string Value)
{
	public static ContactUrlType Profile => new("profile");
	public static ContactUrlType Blog => new("blog");
	public static ContactUrlType Site => new("site");
	public static ContactUrlType Other => new("other");

	public override string ToString() => Value ?? string.Empty;
}
