namespace Freenaute.Freebox.Client.Domains.Files.Support;

internal static class FilesConstants
{
	internal static class Routes
	{
		internal const string Downloads = "downloads/";
		internal const string DownloadsAdd = "downloads/add";
		internal const string DownloadsStats = "downloads/stats";
		internal const string DownloadsConfiguration = "downloads/config/";
		internal const string DownloadsThrottling = "downloads/throttling";
		internal const string DownloadsBlacklist = "downloads/blacklist/";
		internal const string DownloadFeeds = "downloads/feeds/";
		internal const string DownloadFeedsFetch = "downloads/feeds/fetch";
		internal const string FileSystemTasks = "fs/tasks/";
		internal const string FileSystemInfo = "fs/info";
		internal const string FileSystemMove = "fs/mv/";
		internal const string FileSystemCopy = "fs/cp/";
		internal const string FileSystemRemove = "fs/rm/";
		internal const string FileSystemConcatenate = "fs/cat/";
		internal const string FileSystemArchive = "fs/archive/";
		internal const string FileSystemExtract = "fs/extract/";
		internal const string FileSystemRepair = "fs/repair/";
		internal const string FileSystemHash = "fs/hash/";
		internal const string FileSystemCreateDirectory = "fs/mkdir/";
		internal const string FileSystemRename = "fs/rename/";
		internal const string FileSystemListingPrefix = "fs/ls/";
		internal const string DownloadBinaryPrefix = "dl/";
		internal const string Uploads = "upload/";
		internal const string UploadsClean = "upload/clean";
		internal const string ShareLinks = "share_link/";
		internal const string EmptySuffix = "empty/";
		internal const string FetchSuffix = "fetch";
		internal const string DownloadSuffix = "download";
		internal const string MarkAllAsReadSuffix = "mark_all_as_read";
		internal const string LogSuffix = "log";
		internal const string FilesSuffix = "files";
		internal const string PiecesSuffix = "pieces";
		internal const string BlacklistSuffix = "blacklist/";
		internal const string PeersSuffix = "peers/";
		internal const string TrackersSuffix = "trackers/";
		internal const string EraseSuffix = "erase";
		internal const string CancelSuffix = "cancel";
		internal const string HashSuffix = "hash";
		internal const string ItemsSuffix = "items/";
	}

	internal static class Query
	{
		internal const string OnlyFolder = "onlyFolder";
		internal const string CountSubFolder = "countSubFolder";
		internal const string RemoveHidden = "removeHidden";
		internal const string ExifMode = "exifMode";
		internal const string Limit = "limit";
		internal const string Cursor = "cursor";
		internal const string True = "true";
		internal const string False = "false";
		internal const string ExifLight = "light";
		internal const string ExifFull = "full";
		internal const string ExifBase64 = "base64";
	}
}
