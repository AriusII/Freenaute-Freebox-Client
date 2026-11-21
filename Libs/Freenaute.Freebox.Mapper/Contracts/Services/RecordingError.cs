namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for Precord.error; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Precord.error" />
[JsonConverter(typeof(RecordingErrorJsonConverter))]
public readonly record struct RecordingError(string Value)
{
    public static RecordingError None => new("none");
    public static RecordingError FileAccessError => new("file_access_error");
    public static RecordingError DiskFull => new("disk_full");
    public static RecordingError PrivateButNoPrivateDir => new("private_but_no_private_dir");
    public static RecordingError NetworkProblem => new("network_problem");
    public static RecordingError ResourceProblem => new("resource_problem");
    public static RecordingError NoStreamAvailable => new("no_stream_available");
    public static RecordingError NoDataReceived => new("no_data_received");
    public static RecordingError Missed => new("missed");
    public static RecordingError Stopped => new("stopped");
    public static RecordingError InternalError => new("internal_error");
    public static RecordingError UnknownError => new("unknown_error");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
