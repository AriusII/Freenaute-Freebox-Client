using Freenaute.Freebox.Client.Domains.SystemHome.Support;

namespace Freenaute.Freebox.Client.Domains.SystemHome.Lcd;

public sealed class FreeboxLcdApi
{
	private readonly IFreeboxTransport _transport;

	internal FreeboxLcdApi(IFreeboxTransport transport)
	{
		_transport = transport;
	}

	public async Task<LcdConfig> GetAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Get, SystemHomeConstants.Routes.LcdConfiguration, SystemHomeWire.Json.LcdConfig, cancellationToken)
			.ConfigureAwait(false);

	public FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig> Configure()
	{
		return new FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig>(_transport, HttpMethod.Put, SystemHomeConstants.Routes.LcdConfiguration,
			new LcdConfigPatch(), SystemHomeWire.Json.LcdConfigPatch,
			SystemHomeWire.Json.LcdConfig);
	}

	public FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig> WithBrightness(int brightness) =>
		Configure().UseFields(new LcdConfigPatch().WithBrightness(brightness));
}
