namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for Precord.channel_type; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord.channel_type" />
[JsonConverter(typeof(RecordingChannelTypeJsonConverter))]
public readonly record struct RecordingChannelType(string Value)
{
    public static RecordingChannelType Auto => new("");
    public static RecordingChannelType Iptv => new("iptv");
    public static RecordingChannelType Dvb => new("dvb");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
