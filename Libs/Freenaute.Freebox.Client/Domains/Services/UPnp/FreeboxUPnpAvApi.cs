using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.UPnp;

public sealed class FreeboxUPnpAvApi(IFreeboxTransport transport)
{
	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-upnpav-config-" />
	public async Task<UPnpAvConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.UpnpAv.Configuration,
			ServicesJsonSerializerContext.Default.UPnpAvConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-v8-upnpav-config-" />
	public Task<UPnpAvConfig> UpdateConfigurationAsync(UPnpAvConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.UpnpAv.Configuration, request,
			ServicesJsonSerializerContext.Default.UPnpAvConfigPatch, ServicesJsonSerializerContext.Default.UPnpAvConfig,
			cancellationToken);
	}

	public FreeboxServiceCommand<UPnpAvConfigPatch, UPnpAvConfig> Configure()
	{
		return new FreeboxServiceCommand<UPnpAvConfigPatch, UPnpAvConfig>(new UPnpAvConfigPatch(),
			UpdateConfigurationAsync);
	}
}
