namespace Freenaute.Freebox.Client.Domains.Files.Support;

internal static class FilesRequestValidator
{
	public static ArchiveFilesRequest Validate(ArchiveFilesRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Files);
		foreach (var path in request.Files)
		{
			_ = path.Value;
		}

		_ = request.Dst.Value;
		return request;
	}


	public static ConcatenateFilesRequest Validate(ConcatenateFilesRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Files);
		foreach (var path in request.Files)
		{
			_ = path.Value;
		}

		_ = request.Dst.Value;
		RejectNull(request.MultiVolumes, "MultiVolumes");
		RejectNull(request.DeleteFiles, "DeleteFiles");
		RejectNull(request.Overwrite, "Overwrite");
		RejectNull(request.Append, "Append");
		return request;
	}

	public static CreateDirectoryRequest Validate(CreateDirectoryRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		_ = request.Parent.Value;
		ArgumentNullException.ThrowIfNull(request.Dirname);
		return request;
	}

	public static CreateDownloadFeedRequest Validate(CreateDownloadFeedRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Url);
		return request;
	}


	public static CreateShareLinkRequest Validate(CreateShareLinkRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Path);
		return request;
	}


	public static DownloadFilePriorityRequest Validate(DownloadFilePriorityRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return !Enum.IsDefined(request.Priority)
			? throw new ArgumentOutOfRangeException(nameof(request), "Invalid Priority value.")
			: request;
	}

	public static ExtractFilesRequest Validate(ExtractFilesRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		_ = request.Src.Value;
		_ = request.Dst.Value;
		RejectNull(request.Password, "Password");
		RejectNull(request.DeleteArchive, "DeleteArchive");
		RejectNull(request.Overwrite, "Overwrite");
		return request;
	}


	public static FileTransferRequest Validate(FileTransferRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Files);
		foreach (var path in request.Files)
		{
			_ = path.Value;
		}

		_ = request.Dst.Value;
		return !Enum.IsDefined(request.Mode)
			? throw new ArgumentOutOfRangeException(nameof(request), "Invalid Mode value.")
			: request;
	}


	public static HashFileRequest Validate(HashFileRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		_ = request.Src.Value;
		ArgumentNullException.ThrowIfNull(request.HashType);
		return request;
	}

	public static MarkDownloadFeedItemRequest Validate(MarkDownloadFeedItemRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return request;
	}

	public static RemoveFilesRequest Validate(RemoveFilesRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		ArgumentNullException.ThrowIfNull(request.Files);
		foreach (var path in request.Files)
		{
			_ = path.Value;
		}

		return request;
	}

	public static RenameFileRequest Validate(RenameFileRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		_ = request.Src.Value;
		ArgumentNullException.ThrowIfNull(request.Dst);
		return request;
	}

	public static RepairFilesRequest Validate(RepairFilesRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		_ = request.Src.Value;
		RejectNull(request.DeleteArchive, "DeleteArchive");
		return request;
	}

	public static SetDownloadThrottlingRequest Validate(SetDownloadThrottlingRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return !Enum.IsDefined(request.Throttling)
			? throw new ArgumentOutOfRangeException(nameof(request), "Invalid Throttling value.")
			: request;
	}

	public static SetFileTaskStateRequest Validate(SetFileTaskStateRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return !Enum.IsDefined(request.State)
			? throw new ArgumentOutOfRangeException(nameof(request), "Invalid State value.")
			: request;
	}


	public static UpdateDlBtConfig Validate(UpdateDlBtConfig request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.MaxPeers, "MaxPeers");
		RejectNull(request.StopRatio, "StopRatio");
		RejectNull(request.CryptoSupport, "CryptoSupport");
		if (request.CryptoSupport.HasValue && !Enum.IsDefined(request.CryptoSupport.Value))
		{
			throw new ArgumentOutOfRangeException(nameof(request), "Invalid CryptoSupport value.");
		}

		RejectNull(request.EnableDht, "EnableDht");
		RejectNull(request.EnablePex, "EnablePex");
		RejectNull(request.AnnounceTimeout, "AnnounceTimeout");
		RejectNull(request.MainPort, "MainPort");
		RejectNull(request.DhtPort, "DhtPort");
		return request;
	}

	public static UpdateDlFeedConfig Validate(UpdateDlFeedConfig request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.FetchInterval, "FetchInterval");
		RejectNull(request.MaxItems, "MaxItems");
		return request;
	}

	public static UpdateDlNewsConfig Validate(UpdateDlNewsConfig request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.Server, "Server");
		RejectNull(request.Port, "Port");
		RejectNull(request.Ssl, "Ssl");
		RejectNull(request.User, "User");
		RejectNull(request.Password, "Password");
		RejectNull(request.Nthreads, "Nthreads");
		RejectNull(request.AutoRepair, "AutoRepair");
		RejectNull(request.LazyPar2, "LazyPar2");
		RejectNull(request.AutoExtract, "AutoExtract");
		RejectNull(request.EraseTmp, "EraseTmp");
		return request;
	}

	public static UpdateDlRate Validate(UpdateDlRate request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.TxRate, "TxRate");
		RejectNull(request.RxRate, "RxRate");
		return request;
	}

	public static UpdateDlThrottlingConfig Validate(UpdateDlThrottlingConfig request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.Normal, "Normal");
		if (request.Normal.HasValue)
		{
			Validate(request.Normal.Value);
		}

		RejectNull(request.Slow, "Slow");
		if (request.Slow.HasValue)
		{
			Validate(request.Slow.Value);
		}

		RejectNull(request.Schedule, "Schedule");
		if (request.Schedule.HasValue && (request.Schedule.Value.Length is not 168 ||
		                                  request.Schedule.Value.Any(value => !Enum.IsDefined(value))))
		{
			throw new ArgumentException("Schedule must contain 168 documented mode values.", nameof(request));
		}

		RejectNull(request.Mode, "Mode");
		if (request.Mode.HasValue && !Enum.IsDefined(request.Mode.Value))
		{
			throw new ArgumentOutOfRangeException(nameof(request), "Invalid Mode value.");
		}

		return request;
	}

	public static UpdateDownloadConfiguration Validate(UpdateDownloadConfiguration request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.MaxDownloadingTasks, "MaxDownloadingTasks");
		RejectNull(request.DownloadDir, "DownloadDir");
		if (request.DownloadDir.HasValue)
		{
			_ = request.DownloadDir.Value.Value;
		}

		RejectNull(request.WatchDir, "WatchDir");
		if (request.WatchDir.HasValue)
		{
			_ = request.WatchDir.Value.Value;
		}

		RejectNull(request.UseWatchDir, "UseWatchDir");
		RejectNull(request.Throttling, "Throttling");
		if (request.Throttling.HasValue)
		{
			Validate(request.Throttling.Value);
		}

		RejectNull(request.News, "News");
		if (request.News.HasValue)
		{
			Validate(request.News.Value);
		}

		RejectNull(request.Bt, "Bt");
		if (request.Bt.HasValue)
		{
			Validate(request.Bt.Value);
		}

		RejectNull(request.Feed, "Feed");
		if (request.Feed.HasValue)
		{
			Validate(request.Feed.Value);
		}

		RejectNull(request.Dns1, "Dns1");
		RejectNull(request.Dns2, "Dns2");
		return request;
	}

	public static UpdateDownloadFeedRequest Validate(UpdateDownloadFeedRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		return request;
	}

	public static UpdateDownloadRequest Validate(UpdateDownloadRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);
		RejectNull(request.Status, "Status");
		if (request.Status.HasValue && !Enum.IsDefined(request.Status.Value))
		{
			throw new ArgumentOutOfRangeException(nameof(request), "Invalid Status value.");
		}

		RejectNull(request.QueuePos, "QueuePos");
		RejectNull(request.IoPriority, "IoPriority");
		if (request.IoPriority.HasValue && !Enum.IsDefined(request.IoPriority.Value))
		{
			throw new ArgumentOutOfRangeException(nameof(request), "Invalid IoPriority value.");
		}

		RejectNull(request.ArchivePassword, "ArchivePassword");
		return request;
	}


	public static EncodedFreeboxPath[] Validate(EncodedFreeboxPath[] paths)
	{
		ArgumentNullException.ThrowIfNull(paths);
		foreach (var path in paths)
		{
			_ = path.Value;
		}

		return paths;
	}

	private static void RejectNull<T>(Optional<T> value, string name)
	{
		if (value.IsNull)
		{
			throw new ArgumentException("Null clearing is not documented; omit the field instead.", name);
		}
	}
}
