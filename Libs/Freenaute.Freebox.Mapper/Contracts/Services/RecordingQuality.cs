namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for Precord.channel_quality; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord.channel_quality" />
[JsonConverter(typeof(RecordingQualityJsonConverter))]
public readonly record struct RecordingQuality(string Value)
{
    public static RecordingQuality Auto => new("auto");
    public static RecordingQuality Hd => new("hd");
    public static RecordingQuality Sd => new("sd");
    public static RecordingQuality Ld => new("ld");
    public static RecordingQuality ThreeD => new("3d");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
