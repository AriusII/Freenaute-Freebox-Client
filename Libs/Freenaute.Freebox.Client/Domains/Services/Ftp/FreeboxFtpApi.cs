using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Ftp;

public sealed class FreeboxFtpApi(IFreeboxTransport transport)
{
	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#get--api-v8-ftp-config-" />
	public async Task<FtpConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Ftp.Configuration, ServicesJsonSerializerContext.Default.FtpConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#put--api-v8-ftp-config-" />
	public Task<FtpConfig> UpdateConfigurationAsync(FtpConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Ftp.Configuration, request,
			ServicesJsonSerializerContext.Default.FtpConfigPatch, ServicesJsonSerializerContext.Default.FtpConfig,
			cancellationToken);
	}

	public FreeboxServiceCommand<FtpConfigPatch, FtpConfig> Configure() =>
		new(new FtpConfigPatch(), UpdateConfigurationAsync);
}
