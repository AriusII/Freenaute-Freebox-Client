namespace Freenaute.Freebox.Client.Domains.Services.Voicemail;

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

	public async Task<VoicemailEntry> GetAsync(CancellationToken cancellationToken = default) =>
		await _resource.GetAsync(cancellationToken).ConfigureAwait(false);

	public async Task<VoicemailEntry>
		UpdateAsync(VoicemailPatch request, CancellationToken cancellationToken = default) =>
		await _resource.UpdateAsync(request, cancellationToken).ConfigureAwait(false);

	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _resource.DeleteAsync(cancellationToken).ConfigureAwait(false);

	public FreeboxServiceCommand<VoicemailPatch, VoicemailEntry> Configure() => _resource.Configure();

	/// <summary>Downloads the documented WAV response; the caller owns and disposes the download.</summary>
	public async Task<FreeboxDownload> DownloadAudioAsync(CancellationToken cancellationToken = default)
	{
		return await (_binary ??
		              throw new NotSupportedException("The supplied transport does not support binary downloads."))
			.DownloadAsync(HttpMethod.Get, _path + "/audio_file", cancellationToken).ConfigureAwait(false);
	}
}
