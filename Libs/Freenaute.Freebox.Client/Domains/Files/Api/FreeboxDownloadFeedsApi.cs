namespace Freenaute.Freebox.Client.Domains.Files.Api;

public sealed class FreeboxDownloadFeedsApi(IFreeboxTransport transport)
{
	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-feeds-</summary>
	public async Task<DownloadFeed[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.DownloadFeeds,
			FilesJsonSerializerContext.Default.DownloadFeedArray, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-feeds-id</summary>
	public Task<DownloadFeed> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.DownloadFeeds + FilesPath.Id(id),
			FilesJsonSerializerContext.Default.DownloadFeed, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-feeds-</summary>
	public async Task<DownloadFeedMutation> CreateAsync(CreateDownloadFeedRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.DownloadFeeds,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.CreateDownloadFeedRequest,
			FilesJsonSerializerContext.Default.DownloadFeedMutation, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-feeds-id</summary>
	public Task DeleteAsync(long id, CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.DownloadFeeds + FilesPath.Id(id),
			cancellationToken);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-feeds-id</summary>
	public async Task<DownloadFeedMutation> UpdateAsync(long id, UpdateDownloadFeedRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, FilesConstants.Routes.DownloadFeeds + FilesPath.Id(id),
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.UpdateDownloadFeedRequest,
			FilesJsonSerializerContext.Default.DownloadFeedMutation, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-feeds-id-fetch</summary>
	public Task FetchAsync(long id, CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Post,
			$"{FilesConstants.Routes.DownloadFeeds}{FilesPath.Id(id)}/{FilesConstants.Routes.FetchSuffix}",
			cancellationToken);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-feeds-fetch</summary>
	public async Task FetchAllAsync(CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.DownloadFeedsFetch, cancellationToken)
			.ConfigureAwait(false);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-feeds-feed_id-items-</summary>
	public Task<DownloadFeedItem[]> GetItemsAsync(long feedId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.DownloadFeeds}{FilesPath.Id(feedId)}/{FilesConstants.Routes.ItemsSuffix}",
			FilesJsonSerializerContext.Default.DownloadFeedItemArray, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-feeds-feed_id-items-item_id</summary>
	public async Task MarkItemAsync(long feedId, long itemId, MarkDownloadFeedItemRequest request,
		CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Put,
			$"{FilesConstants.Routes.DownloadFeeds}{FilesPath.Id(feedId)}/{FilesConstants.Routes.ItemsSuffix}{FilesPath.Id(itemId)}",
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.MarkDownloadFeedItemRequest,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-feeds-feed_id-items-item_id-download</summary>
	public Task DownloadItemAsync(long feedId, long itemId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post,
			$"{FilesConstants.Routes.DownloadFeeds}{FilesPath.Id(feedId)}/{FilesConstants.Routes.ItemsSuffix}{FilesPath.Id(itemId)}/{FilesConstants.Routes.DownloadSuffix}",
			cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-feeds-feed_id-items-mark_all_as_read</summary>
	public async Task MarkAllAsReadAsync(long feedId, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Post,
			$"{FilesConstants.Routes.DownloadFeeds}{FilesPath.Id(feedId)}/{FilesConstants.Routes.ItemsSuffix}{FilesConstants.Routes.MarkAllAsReadSuffix}",
			cancellationToken).ConfigureAwait(false);
	}
}
