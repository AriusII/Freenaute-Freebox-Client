using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Calls;

public sealed class FreeboxCallApi
{
	private readonly IFreeboxBinaryTransport? _binary;
	private readonly IFreeboxTransport _transport;

	internal FreeboxCallApi(IFreeboxTransport transport, IFreeboxBinaryTransport? binary)
	{
		_transport = transport;
		_binary = binary;
	}

	public async Task<CallEntry[]> GetLogAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Calls.Log,
			ServicesJsonSerializerContext.Default.CallEntryArray,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task DeleteAllLogEntriesAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Calls.DeleteAll, cancellationToken).ConfigureAwait(false);

	public async Task MarkAllLogEntriesAsReadAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Calls.MarkAllAsRead, cancellationToken)
			.ConfigureAwait(false);

	public FreeboxServiceResource<CallEntry, CallEntryPatch> LogEntry(long id)
	{
		return new FreeboxServiceResource<CallEntry, CallEntryPatch>(_transport, $"call/log/{ServicesPath.Id(id)}",
			ServicesJsonSerializerContext.Default.CallEntry,
			ServicesJsonSerializerContext.Default.CallEntryPatch, request => request.Validate());
	}

	public async Task<CallEntry> GetLogEntryAsync(long id, CancellationToken cancellationToken = default) =>
		await LogEntry(id).GetAsync(cancellationToken).ConfigureAwait(false);

	public async Task<CallEntry> UpdateLogEntryAsync(long id, CallEntryPatch request,
		CancellationToken cancellationToken = default) =>
		await LogEntry(id).UpdateAsync(request, cancellationToken).ConfigureAwait(false);

	public async Task DeleteLogEntryAsync(long id, CancellationToken cancellationToken = default) =>
		await LogEntry(id).DeleteAsync(cancellationToken).ConfigureAwait(false);

	public async Task<CallAccount> GetAccountAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Calls.Account,
			ServicesJsonSerializerContext.Default.CallAccount,
			cancellationToken).ConfigureAwait(false);
	}

	public async Task<VoicemailEntry[]> GetVoicemailsAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Calls.Voicemail,
			ServicesJsonSerializerContext.Default.VoicemailEntryArray, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxVoicemail Voicemail(string id) => new(_transport, _binary, ServicesPath.Segment(id));
}
