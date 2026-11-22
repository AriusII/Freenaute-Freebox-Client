namespace Freenaute.Freebox.Client.Domains.Files.Api;

public sealed class FreeboxSharesApi(IFreeboxTransport transport)
{
	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-share_link-</summary>
	public async Task<ShareLink[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.ShareLinks,
			FilesJsonSerializerContext.Default.ShareLinkArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-share_link-</summary>
	public Task<ShareLink> CreateAsync(CreateShareLinkRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.ShareLinks,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.CreateShareLinkRequest, FilesJsonSerializerContext.Default.ShareLink,
			cancellationToken);
	}

	public async Task<ShareLink> GetAsync(string token, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.ShareLinks + FilesPath.Opaque(token),
			FilesJsonSerializerContext.Default.ShareLink, cancellationToken).ConfigureAwait(false);
	}

	public async Task DeleteAsync(string token, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.ShareLinks + FilesPath.Opaque(token),
				cancellationToken)
			.ConfigureAwait(false);
}
