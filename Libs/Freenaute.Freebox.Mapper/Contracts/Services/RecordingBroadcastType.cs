namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for Precord.broadcast_type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord.broadcast_type" />
[JsonConverter(typeof(RecordingBroadcastTypeJsonConverter))]
public readonly record struct RecordingBroadcastType(string Value)
{
    public static RecordingBroadcastType Tv => new("tv");
    public static RecordingBroadcastType Radio => new("radio");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
