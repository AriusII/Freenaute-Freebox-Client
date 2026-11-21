namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxCallApi
{
	private readonly IFreeboxBinaryTransport? _binary;
	private readonly IFreeboxTransport _transport;

	internal FreeboxCallApi(IFreeboxTransport transport, IFreeboxBinaryTransport? binary)
	{
		_transport = transport;
		_binary = binary;
	}

	public Task<CallEntry[]> GetLogAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, "call/log/", ServicesJsonSerializerContext.Default.CallEntryArray,
			cancellationToken);
	}

	public Task DeleteAllLogEntriesAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Post, "call/log/delete_all/", cancellationToken);

	public Task MarkAllLogEntriesAsReadAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Post, "call/log/mark_all_as_read/", cancellationToken);

	public FreeboxServiceResource<CallEntry, CallEntryPatch> LogEntry(long id)
	{
		return new FreeboxServiceResource<CallEntry, CallEntryPatch>(_transport, $"call/log/{ServicesPath.Id(id)}",
			ServicesJsonSerializerContext.Default.CallEntry,
			ServicesJsonSerializerContext.Default.CallEntryPatch, request => request.Validate());
	}

	public Task<CallAccount> GetAccountAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, "call/account", ServicesJsonSerializerContext.Default.CallAccount,
			cancellationToken);
	}

	public Task<VoicemailEntry[]> GetVoicemailsAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, "call/voicemail/",
			ServicesJsonSerializerContext.Default.VoicemailEntryArray, cancellationToken);
	}

	public FreeboxVoicemail Voicemail(string id) => new(_transport, _binary, ServicesPath.Segment(id));
}
