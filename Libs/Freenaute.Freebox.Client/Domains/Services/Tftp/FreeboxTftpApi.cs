using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Tftp;

public sealed class FreeboxTftpApi(IFreeboxTransport transport)
{
	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v16-tftp-config-" />
	public async Task<TftpConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Tftp.Configuration,
			ServicesJsonSerializerContext.Default.TftpConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-latest-tftp-config-" />
	public Task<TftpConfig> UpdateConfigurationAsync(TftpConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Tftp.Configuration, request,
			ServicesJsonSerializerContext.Default.TftpConfigPatch, ServicesJsonSerializerContext.Default.TftpConfig,
			cancellationToken);
	}

	public FreeboxServiceCommand<TftpConfigPatch, TftpConfig> Configure() =>
		new(new TftpConfigPatch(), UpdateConfigurationAsync);
}
