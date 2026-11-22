using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Language;

public sealed class FreeboxLanguageApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxLanguageApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<LanguageSupport> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.Language, SystemHomeWire.Json.LanguageSupport, cancellationToken)
			.ConfigureAwait(false);

	public FreeboxSystemHomeCommand<SetLanguageRequest> Set(string language)
	{
		return new FreeboxSystemHomeCommand<SetLanguageRequest>(_transport, HttpMethod.Post, SystemHomeConstants.Routes.Language,
			new SetLanguageRequest(language), SystemHomeWire.Json.SetLanguageRequest);
	}
}
