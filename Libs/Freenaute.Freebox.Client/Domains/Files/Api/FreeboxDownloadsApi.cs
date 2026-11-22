namespace Freenaute.Freebox.Client.Domains.Files.Api;

public sealed class FreeboxDownloadsApi(IFreeboxTransport transport)
{
	public FreeboxDownloadFeedsApi Feeds { get; } = new(transport);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-</summary>
	public async Task<ObjectOrArray<Download>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.Downloads,
			FilesJsonSerializerContext.Default.ObjectOrArrayDownload, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-id</summary>
	public Task<Download> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.Downloads + FilesPath.Id(id),
			FilesJsonSerializerContext.Default.Download, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-id</summary>
	public async Task DeleteAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.Downloads + FilesPath.Id(id),
				cancellationToken)
			.ConfigureAwait(false);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-id-erase</summary>
	public Task EraseAsync(long id, CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Delete,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(id)}/{FilesConstants.Routes.EraseSuffix}",
			cancellationToken);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-id</summary>
	public async Task<Download> UpdateAsync(long id, UpdateDownloadRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, FilesConstants.Routes.Downloads + FilesPath.Id(id),
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.UpdateDownloadRequest,
			FilesJsonSerializerContext.Default.Download, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-id-log</summary>
	public Task<string> GetLogAsync(long id, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(id)}/{FilesConstants.Routes.LogSuffix}",
			FilesJsonSerializerContext.Default.String, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-stats</summary>
	public async Task<DownloadStats> GetStatisticsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.DownloadsStats,
			FilesJsonSerializerContext.Default.DownloadStats,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-task_id-files</summary>
	public Task<DownloadFile[]> GetFilesAsync(long taskId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.FilesSuffix}",
			FilesJsonSerializerContext.Default.DownloadFileArray, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-task_id-files-file_id</summary>
	public async Task SetFilePriorityAsync(long taskId, string fileId, DownloadFilePriorityRequest request,
		CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Put,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.FilesSuffix}/{FilesPath.Opaque(fileId)}",
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.DownloadFilePriorityRequest,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-task_id-pieces</summary>
	public Task<string> GetPiecesAsync(long taskId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.PiecesSuffix}",
			FilesJsonSerializerContext.Default.String, cancellationToken);
	}

	/// <summary>Get one download blacklist.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-task_id-blacklist
	/// </remarks>
	public async Task<DownloadBlacklistEntry[]> GetBlacklistAsync(long taskId,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.BlacklistSuffix}",
			FilesJsonSerializerContext.Default.DownloadBlacklistEntryArray, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Clear one download blacklist.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-task_id-blacklist-empty
	/// </remarks>
	public Task ClearBlacklistAsync(long taskId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Delete,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.BlacklistSuffix}{FilesConstants.Routes.EmptySuffix}",
			cancellationToken);
	}

	/// <summary>Add one host entry to the global downloader blacklist.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-blacklist
	/// </remarks>
	public async Task<DownloadBlacklistEntry> AddBlacklistHostAsync(AddDownloadBlacklistEntryRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.DownloadsBlacklist,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.AddDownloadBlacklistEntryRequest,
			FilesJsonSerializerContext.Default.DownloadBlacklistEntry,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Remove one host entry from the global downloader blacklist.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-blacklist-host-
	/// </remarks>
	public Task RemoveBlacklistHostAsync(string host, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Delete,
			$"{FilesConstants.Routes.DownloadsBlacklist}{FilesPath.Opaque(host)}/",
			cancellationToken);
	}

	/// <summary>Get peer details for one download.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-task_id-peers
	/// </remarks>
	public async Task<DownloadPeer[]> GetPeersAsync(long taskId, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.PeersSuffix}",
			FilesJsonSerializerContext.Default.DownloadPeerArray, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get tracker details for one download.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-task_id-trackers
	/// </remarks>
	public Task<DownloadTracker[]> GetTrackersAsync(long taskId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.TrackersSuffix}",
			FilesJsonSerializerContext.Default.DownloadTrackerArray, cancellationToken);
	}

	/// <summary>Add one tracker to one download.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#post--api-v8-downloads-task_id-trackers
	/// </remarks>
	public async Task AddTrackerAsync(long taskId, AddDownloadTrackerRequest request,
		CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Post,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.TrackersSuffix}",
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.AddDownloadTrackerRequest,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one tracker entry.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-task_id-trackers-announce-
	/// </remarks>
	public Task UpdateTrackerAsync(long taskId, string announce, UpdateDownloadTrackerRequest request,
		CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Put,
			$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.TrackersSuffix}{FilesPath.Opaque(announce)}/",
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.UpdateDownloadTrackerRequest,
			cancellationToken);
	}

	/// <summary>Delete one tracker entry.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-downloads-task_id-trackers-announce-
	/// </remarks>
	public async Task DeleteTrackerAsync(long taskId, string announce, CancellationToken cancellationToken = default)
	{
		await transport.SendAsync(HttpMethod.Delete,
				$"{FilesConstants.Routes.Downloads}{FilesPath.Id(taskId)}/{FilesConstants.Routes.TrackersSuffix}{FilesPath.Opaque(announce)}/",
				cancellationToken)
			.ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-downloads-config-</summary>
	public Task<DownloadConfiguration> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.DownloadsConfiguration,
			FilesJsonSerializerContext.Default.DownloadConfiguration, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-config-</summary>
	public async Task<DownloadConfiguration> UpdateConfigurationAsync(UpdateDownloadConfiguration request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, FilesConstants.Routes.DownloadsConfiguration,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.UpdateDownloadConfiguration,
			FilesJsonSerializerContext.Default.DownloadConfiguration, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v8-downloads-throttling</summary>
	public Task<DownloadThrottlingStatus> SetThrottlingAsync(SetDownloadThrottlingRequest request,
		CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Put, FilesConstants.Routes.DownloadsThrottling,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.SetDownloadThrottlingRequest,
			FilesJsonSerializerContext.Default.DownloadThrottlingStatus, cancellationToken);
	}

	public FreeboxDownloadSubmission FromUrl(string url) => new(this, new DownloadUrlRequest { DownloadUrl = url });

	public FreeboxDownloadSubmission FromUrls(params string[] urls)
	{
		ArgumentNullException.ThrowIfNull(urls);
		return new FreeboxDownloadSubmission(this, new DownloadUrlRequest { DownloadUrls = urls });
	}

	public async Task<AddedDownloads> AddUrlAsync(DownloadUrlRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendContentAsync(HttpMethod.Post, FilesConstants.Routes.DownloadsAdd,
			FilesForm.Create(request),
			FilesJsonSerializerContext.Default.AddedDownloads, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Upload a torrent/NZB download descriptor, not a filesystem file-upload operation.</summary>
	public Task<AddedDownloads> AddFileAsync(Stream file, string fileName, EncodedFreeboxPath? downloadDirectory = null,
		string? archivePassword = null, bool leaveOpen = true, CancellationToken cancellationToken = default)
	{
		return transport.SendContentAsync(HttpMethod.Post, FilesConstants.Routes.DownloadsAdd,
			FilesForm.CreateMultipart(file, fileName, downloadDirectory, archivePassword, leaveOpen),
			FilesJsonSerializerContext.Default.AddedDownloads, cancellationToken);
	}
}
