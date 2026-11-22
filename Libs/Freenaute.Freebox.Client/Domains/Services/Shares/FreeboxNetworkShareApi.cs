using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Shares;

public sealed class FreeboxNetworkShareApi(IFreeboxTransport transport)
{
	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-netshare-samba-" />
	public async Task<SambaConfig> GetSambaConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Shares.Samba,
			ServicesJsonSerializerContext.Default.SambaConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-v8-netshare-samba-" />
	public Task<SambaConfig> UpdateSambaConfigurationAsync(SambaConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Shares.Samba, request,
			ServicesJsonSerializerContext.Default.SambaConfigPatch, ServicesJsonSerializerContext.Default.SambaConfig,
			cancellationToken);
	}

	public FreeboxServiceCommand<SambaConfigPatch, SambaConfig> ConfigureSamba()
	{
		return new FreeboxServiceCommand<SambaConfigPatch, SambaConfig>(new SambaConfigPatch(),
			UpdateSambaConfigurationAsync);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-netshare-afp-" />
	public async Task<AfpConfig> GetAfpConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Shares.Afp,
			ServicesJsonSerializerContext.Default.AfpConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-v8-netshare-afp-" />
	public Task<AfpConfig> UpdateAfpConfigurationAsync(AfpConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Shares.Afp, request,
			ServicesJsonSerializerContext.Default.AfpConfigPatch, ServicesJsonSerializerContext.Default.AfpConfig,
			cancellationToken);
	}

	public FreeboxServiceCommand<AfpConfigPatch, AfpConfig> ConfigureAfp() =>
		new(new AfpConfigPatch(), UpdateAfpConfigurationAsync);
}
