namespace Freenaute.Freebox.Client.Domains.Files.Api;

[Obsolete("Deprecated by Freebox API documentation. Prefer WebSocket upload via WebSockets.Uploads (/ws/upload).")]
public sealed class FreeboxFileUploadsApi(IFreeboxTransport transport)
{
	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-upload-</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	public async Task<FileUpload[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.Uploads,
			FilesJsonSerializerContext.Default.FileUploadArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-upload-id</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	public async Task<FileUpload> GetAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, FilesConstants.Routes.Uploads + FilesPath.Id(id),
			FilesJsonSerializerContext.Default.FileUpload, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-upload-id-cancel</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	public async Task CancelAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete,
				$"{FilesConstants.Routes.Uploads}{FilesPath.Id(id)}/{FilesConstants.Routes.CancelSuffix}",
				cancellationToken)
			.ConfigureAwait(false);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-upload-id</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	public async Task DeleteAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.Uploads + FilesPath.Id(id),
				cancellationToken)
			.ConfigureAwait(false);

	/// <summary>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-upload-clean</summary>
	[Obsolete("Deprecated by Freebox API documentation.")]
	public async Task DeleteCleanAsync(CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, FilesConstants.Routes.UploadsClean, cancellationToken)
			.ConfigureAwait(false);
}
