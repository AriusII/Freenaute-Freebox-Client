namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxVoicemail
{
	private readonly IFreeboxBinaryTransport? _binary;
	private readonly string _path;
	private readonly FreeboxServiceResource<VoicemailEntry, VoicemailPatch> _resource;

	internal FreeboxVoicemail(IFreeboxTransport transport, IFreeboxBinaryTransport? binary, string id)
	{
		_binary = binary;
		_path = $"call/voicemail/{id}";
		_resource = new FreeboxServiceResource<VoicemailEntry, VoicemailPatch>(transport, _path,
			ServicesJsonSerializerContext.Default.VoicemailEntry,
			ServicesJsonSerializerContext.Default.VoicemailPatch, request => request.Validate());
	}

	public Task<VoicemailEntry> GetAsync(CancellationToken cancellationToken = default) =>
		_resource.GetAsync(cancellationToken);

	public Task<VoicemailEntry> UpdateAsync(VoicemailPatch request, CancellationToken cancellationToken = default) =>
		_resource.UpdateAsync(request, cancellationToken);

	public Task DeleteAsync(CancellationToken cancellationToken = default) => _resource.DeleteAsync(cancellationToken);

	public FreeboxServiceCommand<VoicemailPatch, VoicemailEntry> Configure() => _resource.Configure();

	/// <summary>Downloads the documented WAV response; the caller owns and disposes the download.</summary>
	public Task<FreeboxDownload> DownloadAudioAsync(CancellationToken cancellationToken = default)
	{
		return (_binary ?? throw new NotSupportedException("The supplied transport does not support binary downloads."))
			.DownloadAsync(HttpMethod.Get, _path + "/audio_file", cancellationToken);
	}
}
