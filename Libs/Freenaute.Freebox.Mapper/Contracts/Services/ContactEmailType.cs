namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for ContactEmail.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#ContactEmail.type" />
[JsonConverter(typeof(ContactEmailTypeJsonConverter))]
public readonly record struct ContactEmailType(string Value)
{
    public static ContactEmailType Home => new("home");
    public static ContactEmailType Work => new("work");
    public static ContactEmailType Other => new("other");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
