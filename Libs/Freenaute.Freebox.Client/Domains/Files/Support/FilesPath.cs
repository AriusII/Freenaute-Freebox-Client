namespace Freenaute.Freebox.Client.Domains.Files.Support;

internal static class FilesPath
{
	private const string InvalidOpaqueSegmentError = "Invalid opaque segment.";
	private const string InvalidExifModeError = "EXIF mode must be light, full or base64.";

	public static string Id(long id)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(id);
		return id.ToString(CultureInfo.InvariantCulture);
	}

	public static string Opaque(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);
		if (value is "." or ".." || value.Any(char.IsControl))
		{
			throw new ArgumentException(InvalidOpaqueSegmentError, nameof(value));
		}

		return Uri.EscapeDataString(value);
	}

	public static string Listing(EncodedFreeboxPath path, FileListingOptions? options)
	{
		string resource = FilesConstants.Routes.FileSystemListingPrefix + path.ToEscapedSegment();
		if (options is null)
		{
			return resource;
		}

		List<KeyValuePair<string, string>> values = [];
		Add(values, FilesConstants.Query.OnlyFolder, options.OnlyFolders);
		Add(values, FilesConstants.Query.CountSubFolder, options.CountSubFolders);
		Add(values, FilesConstants.Query.RemoveHidden, options.RemoveHidden);
		if (options.ExifMode is not null)
		{
			if (options.ExifMode is not FilesConstants.Query.ExifLight and not FilesConstants.Query.ExifFull and
			    not FilesConstants.Query.ExifBase64)
			{
				throw new ArgumentException(InvalidExifModeError, nameof(options));
			}

			values.Add(new KeyValuePair<string, string>(FilesConstants.Query.ExifMode, options.ExifMode));
		}

		if (options.Limit is not null)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(options.Limit.Value);
			values.Add(new KeyValuePair<string, string>(FilesConstants.Query.Limit,
				options.Limit.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (options.Cursor is not null)
		{
			values.Add(new KeyValuePair<string, string>(FilesConstants.Query.Cursor, options.Cursor));
		}

		return Query(resource, values);
	}


	private static void Add(List<KeyValuePair<string, string>> values, string name, bool? value)
	{
		if (value is not null)
		{
			values.Add(new KeyValuePair<string, string>(name,
				value.Value ? FilesConstants.Query.True : FilesConstants.Query.False));
		}
	}

	private static string Query(string path, IEnumerable<KeyValuePair<string, string>> values)
	{
		string query = string.Join('&',
			values.Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value)}"));
		return query.Length is 0 ? path : path + '?' + query;
	}
}
