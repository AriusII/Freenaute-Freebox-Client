namespace Freenaute.Freebox.Mapper.Contracts.Services.Players;

/// <summary>Read projection of the documented PlayerStatusCapabilities wire object.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#PlayerStatusCapabilities" />
public sealed record PlayerStatusCapabilities
{
	[JsonPropertyName("play")] public bool? Play { get; init; }

	[JsonPropertyName("pause")] public bool? Pause { get; init; }

	[JsonPropertyName("stop")] public bool? Stop { get; init; }

	[JsonPropertyName("next")] public bool? Next { get; init; }

	[JsonPropertyName("prev")] public bool? Prev { get; init; }

	[JsonPropertyName("record")] public bool? Record { get; init; }

	[JsonPropertyName("record_stop")] public bool? RecordStop { get; init; }

	[JsonPropertyName("seek_forward")] public bool? SeekForward { get; init; }

	[JsonPropertyName("seek_backward")] public bool? SeekBackward { get; init; }

	[JsonPropertyName("seek_to")] public bool? SeekTo { get; init; }

	[JsonPropertyName("shuffle")] public bool? Shuffle { get; init; }

	[JsonPropertyName("repeat_all")] public bool? RepeatAll { get; init; }

	[JsonPropertyName("repeat_one")] public bool? RepeatOne { get; init; }

	[JsonPropertyName("select_stream")] public bool? SelectStream { get; init; }

	[JsonPropertyName("select_audio_track")]
	public bool? SelectAudioTrack { get; init; }

	[JsonPropertyName("select_srt_track")] public bool? SelectSrtTrack { get; init; }

	public override string ToString() => nameof(PlayerStatusCapabilities);
}
