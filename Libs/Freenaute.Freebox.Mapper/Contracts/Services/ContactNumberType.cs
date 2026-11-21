namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for ContactNumber.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactNumber.type" />
[JsonConverter(typeof(ContactNumberTypeJsonConverter))]
public readonly record struct ContactNumberType(string Value)
{
    public static ContactNumberType Fixed => new("fixed");
    public static ContactNumberType Mobile => new("mobile");
    public static ContactNumberType Work => new("work");
    public static ContactNumberType Fax => new("fax");
    public static ContactNumberType Other => new("other");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
