using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Storage;

/// <summary>Storage API routes.</summary>
public sealed class FreeboxStorageApi(IFreeboxTransport transport)
{
	/// <summary>Get storage configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-storage-config-
	/// </remarks>
	public async Task<StorageConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Storage.Config,
			FilesJsonSerializerContext.Default.StorageConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update storage configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-storage-config-
	/// </remarks>
	public async Task<StorageConfig> UpdateConfigurationAsync(StorageConfig request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Storage.Config, request,
			FilesJsonSerializerContext.Default.StorageConfig,
			FilesJsonSerializerContext.Default.StorageConfig, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List disks.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-storage-disk-
	/// </remarks>
	public async Task<StorageDisk[]> GetDisksAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Storage.Disks,
			FilesJsonSerializerContext.Default.StorageDiskArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one disk.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-storage-disk-id
	/// </remarks>
	public async Task<StorageDisk> GetDiskAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Storage.Disk(FilesPath.Id(id)),
			FilesJsonSerializerContext.Default.StorageDisk,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one disk state.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-storage-disk-id
	/// </remarks>
	public async Task<StorageDisk> SetDiskStateAsync(long id, SetStorageDiskStateRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		RequireText(request.State, nameof(request.State));
		if (request.State is not "error" and not "disabled" and not "enabled" and not "formatting")
		{
			throw new ArgumentException("Disk state must be one of: error, disabled, enabled, formatting.",
				nameof(request));
		}

		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Storage.Disk(FilesPath.Id(id)), request,
			FilesJsonSerializerContext.Default.SetStorageDiskStateRequest,
			FilesJsonSerializerContext.Default.StorageDisk,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Format one disk.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-storage-disk-id-format-
	/// </remarks>
	public async Task FormatDiskAsync(long id, FormatStorageDiskRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		RequireText(request.TableType, nameof(request.TableType));
		RequireText(request.FsType, nameof(request.FsType));
		RequireText(request.Label, nameof(request.Label));
		if (request.TableType is not "msdos" and not "gpt" and not "superfloppy" and not "empty")
		{
			throw new ArgumentException("TableType must be one of: msdos, gpt, superfloppy, empty.", nameof(request));
		}

		if (request.FsType is not "empty" and not "unknown" and not "xfs" and not "ext4" and not "vfat"
		    and not "ntf" and not "hf" and not "hfsplus" and not "swap" and not "exfat")
		{
			throw new ArgumentException("FsType is not a documented filesystem type.", nameof(request));
		}

		await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Storage.DiskFormat(FilesPath.Id(id)), request,
			FilesJsonSerializerContext.Default.FormatStorageDiskRequest, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List partitions.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-storage-partition-
	/// </remarks>
	public async Task<DiskPartition[]> GetPartitionsAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Storage.Partitions,
			FilesJsonSerializerContext.Default.DiskPartitionArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one partition.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#get--api-v8-storage-partition-id
	/// </remarks>
	public async Task<DiskPartition> GetPartitionAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Storage.Partition(FilesPath.Id(id)),
			FilesJsonSerializerContext.Default.DiskPartition,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one partition state.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-storage-partition-id
	/// </remarks>
	public async Task<DiskPartition> SetPartitionStateAsync(long id, SetStoragePartitionStateRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		RequireText(request.State, nameof(request.State));
		if (request.State is not "error" and not "checking" and not "formatting" and not "mounting"
		    and not "maintenance" and not "mounted" and not "umounting" and not "umounted" and not "ejecting")
		{
			throw new ArgumentException("Partition state is not a documented storage state.", nameof(request));
		}

		return await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Storage.Partition(FilesPath.Id(id)), request,
			FilesJsonSerializerContext.Default.SetStoragePartitionStateRequest,
			FilesJsonSerializerContext.Default.DiskPartition, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Run a partition check.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     http://mafreebox.freebox.fr/doc/index.html#put--api-v8-storage-partition-id-check-
	/// </remarks>
	public async Task CheckPartitionAsync(long id, CheckStoragePartitionRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		RequireText(request.CheckMode, nameof(request.CheckMode));
		if (request.CheckMode is not "ro" and not "rw")
		{
			throw new ArgumentException("CheckMode must be ro or rw.", nameof(request));
		}

		await transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Storage.PartitionCheck(FilesPath.Id(id)), request,
			FilesJsonSerializerContext.Default.CheckStoragePartitionRequest, cancellationToken).ConfigureAwait(false);
	}

	private static void RequireText(string? value, string parameterName)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("Value cannot be null, empty, or whitespace.", parameterName);
		}
	}
}
