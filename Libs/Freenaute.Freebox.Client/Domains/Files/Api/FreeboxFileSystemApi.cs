namespace Freenaute.Freebox.Client.Domains.Files.Api;

public sealed class FreeboxFileSystemApi(IFreeboxTransport transport, IFreeboxBinaryTransport binaryTransport)
{
	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v15-fs-tasks-</summary>
	public async Task<FsTask[]> GetTasksAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.FileSystemTasks,
			FilesJsonSerializerContext.Default.FsTaskArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v15-fs-tasks-id</summary>
	public async Task<FsTask> GetTaskAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.FileSystemTasks + FilesPath.Id(id),
			FilesJsonSerializerContext.Default.FsTask, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v15-fs-tasks-id</summary>
	public Task DeleteTaskAsync(long id, CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.FileSystemTasks + FilesPath.Id(id),
			cancellationToken);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#put--api-v15-fs-tasks-id</summary>
	public async Task<FsTask> SetTaskStateAsync(long id, SetFileTaskStateRequest request,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, FilesConstants.Routes.FileSystemTasks + FilesPath.Id(id),
			FilesRequestValidator.Validate(request), FilesJsonSerializerContext.Default.SetFileTaskStateRequest,
			FilesJsonSerializerContext.Default.FsTask, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-info</summary>
	public Task<FileEntry[]> GetInformationAsync(EncodedFreeboxPath[] request,
		CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemInfo,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.EncodedFreeboxPathArray,
			FilesJsonSerializerContext.Default.FileEntryArray, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-mv-</summary>
	public async Task<FsTask> MoveAsync(FileTransferRequest request, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemMove,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.FileTransferRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-cp-</summary>
	public Task<FsTask> CopyAsync(FileTransferRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemCopy,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.FileTransferRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-rm-</summary>
	public async Task<FsTask> RemoveAsync(RemoveFilesRequest request, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemRemove,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.RemoveFilesRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-cat-</summary>
	public Task<FsTask> ConcatenateAsync(ConcatenateFilesRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemConcatenate,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.ConcatenateFilesRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-archive-</summary>
	public async Task<FsTask> ArchiveAsync(ArchiveFilesRequest request, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemArchive,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.ArchiveFilesRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-extract-</summary>
	public Task<FsTask> ExtractAsync(ExtractFilesRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemExtract,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.ExtractFilesRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-repair-</summary>
	public async Task<FsTask> RepairAsync(RepairFilesRequest request, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemRepair,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.RepairFilesRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-hash-</summary>
	public Task<FsTask> HashAsync(HashFileRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemHash,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.HashFileRequest, FilesJsonSerializerContext.Default.FsTask,
			cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v15-fs-tasks-id-hash</summary>
	public async Task<FileHash> GetHashAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.FileSystemTasks}{FilesPath.Id(id)}/{FilesConstants.Routes.HashSuffix}",
			FilesJsonSerializerContext.Default.FileHash, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-mkdir-</summary>
	public Task CreateDirectoryAsync(CreateDirectoryRequest request, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemCreateDirectory,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.CreateDirectoryRequest, cancellationToken);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#post--api-v15-fs-rename-</summary>
	public async Task<string> RenameAsync(RenameFileRequest request, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Post, FilesConstants.Routes.FileSystemRename,
			FilesRequestValidator.Validate(request),
			FilesJsonSerializerContext.Default.RenameFileRequest, FilesJsonSerializerContext.Default.String,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Returns an owned binary response. Dispose it after consuming Content; no range/retry is inferred.</summary>
	public Task<FreeboxDownload>
		DownloadAsync(EncodedFreeboxPath path, CancellationToken cancellationToken = default) =>
		binaryTransport.DownloadAsync(HttpMethod.Get,
			FilesConstants.Routes.DownloadBinaryPrefix + path.ToEscapedSegment(), cancellationToken);

	public FreeboxFileResource At(EncodedFreeboxPath path) => new(this, path);

	public async Task<FileListing> ListAsync(EncodedFreeboxPath path, FileListingOptions? options = null,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesPath.Listing(path, options),
			FilesJsonSerializerContext.Default.FileListing, cancellationToken).ConfigureAwait(false);
	}

	public async Task<FileEntry> GetInformationAsync(EncodedFreeboxPath path,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get,
			$"{FilesConstants.Routes.FileSystemInfo}/{path.ToEscapedSegment()}",
			FilesJsonSerializerContext.Default.FileEntry, cancellationToken).ConfigureAwait(false);
	}
}
