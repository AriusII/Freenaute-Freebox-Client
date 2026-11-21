namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for Precord.state; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord.state" />
[JsonConverter(typeof(RecordingStateJsonConverter))]
public readonly record struct RecordingState(string Value)
{
    public static RecordingState Disabled => new("disabled");
    public static RecordingState StartError => new("start_error");
    public static RecordingState WaitingStartTime => new("waiting_start_time");
    public static RecordingState Starting => new("starting");
    public static RecordingState Running => new("running");
    public static RecordingState RunningError => new("running_error");
    public static RecordingState Failed => new("failed");
    public static RecordingState Finished => new("finished");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
