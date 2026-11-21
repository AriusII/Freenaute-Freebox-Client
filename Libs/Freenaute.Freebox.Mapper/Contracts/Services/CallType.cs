namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for CallEntry.type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#CallEntry.type" />
[JsonConverter(typeof(CallTypeJsonConverter))]
public readonly record struct CallType(string Value)
{
    public static CallType Missed => new("missed");
    public static CallType Accepted => new("accepted");
    public static CallType Outgoing => new("outgoing");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
