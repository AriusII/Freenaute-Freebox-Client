namespace Freenaute.Freebox.Client.Http;

/// <summary>Utilities for constructing API paths without interpreting user input as a route.</summary>
internal static class FreeboxApiPath
{
	private const string RelativeSegmentError = "A route parameter must be one path segment.";
	private const string OpaqueSegmentError = "An opaque parameter cannot contain traversal or control characters.";
	private const string BasePathQueryError = "The API base path cannot contain a query.";
	private const string RelativePathError = "An API path must be relative to the versioned API root.";
	private const string ResourcePathError = "An API path must contain a resource path.";

	private const string TraversalPathError =
		"An API path cannot contain traversal, leading encoded separators or control characters.";

	private const string RootEscapeError = "An API path must stay within the versioned API root.";
	private const string RootSlash = "/";

	public static string EncodeSegment(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);
		if (value is "." or ".." || value.IndexOfAny(['/', '\\']) >= 0)
		{
			throw new ArgumentException(RelativeSegmentError, nameof(value));
		}

		return Uri.EscapeDataString(value);
	}

	/// <summary>Encodes an opaque parameter while preserving separators inside that parameter.</summary>
	public static string EncodeOpaqueSegment(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);
		if (value is "." or ".." || value[0] is '/' or '\\' ||
		    value.Contains('\\', StringComparison.OrdinalIgnoreCase) || value.Any(char.IsControl) ||
		    value.Split('/').Any(part => part is "." or ".."))
		{
			throw new ArgumentException(OpaqueSegmentError, nameof(value));
		}

		return Uri.EscapeDataString(value);
	}

	internal static string NormalizeBasePath(string path)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);
		string relative = path.StartsWith('/', StringComparison.OrdinalIgnoreCase) ? path[1..] : path;
		ValidateRelativePath(relative);
		if (relative.Contains('?', StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException(BasePathQueryError, nameof(path));
		}

		return RootSlash + relative.TrimEnd('/') + RootSlash;
	}

	internal static void ValidateRelativePath(string relativePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
		if (relativePath.StartsWith('/', StringComparison.OrdinalIgnoreCase) ||
		    relativePath.Contains('\\', StringComparison.OrdinalIgnoreCase) ||
		    relativePath.Contains('#', StringComparison.OrdinalIgnoreCase) ||
		    Uri.TryCreate(relativePath, UriKind.Absolute, out _))
		{
			throw new ArgumentException(RelativePathError, nameof(relativePath));
		}

		int queryIndex = relativePath.IndexOf('?', StringComparison.OrdinalIgnoreCase);
		string path = queryIndex < 0 ? relativePath : relativePath[..queryIndex];
		if (path.Length is 0)
		{
			throw new ArgumentException(ResourcePathError, nameof(relativePath));
		}

		foreach (string segment in path.Split('/'))
		{
			string decoded = Uri.UnescapeDataString(segment);
			bool hasTraversalSegment = false;
			foreach (string part in decoded.Split('/'))
			{
				if (part is "." or "..")
				{
					hasTraversalSegment = true;
					break;
				}
			}

			bool hasControlCharacter = false;
			foreach (char character in decoded)
			{
				if (!char.IsControl(character))
				{
					continue;
				}

				hasControlCharacter = true;
				break;
			}

			if (decoded is "." or ".." || decoded.StartsWith('/', StringComparison.OrdinalIgnoreCase) ||
			    decoded.Contains('\\', StringComparison.OrdinalIgnoreCase) ||
			    hasTraversalSegment || hasControlCharacter)
			{
				throw new ArgumentException(TraversalPathError, nameof(relativePath));
			}
		}
	}

	internal static Uri Resolve(Uri apiAddress, string relativePath)
	{
		ValidateRelativePath(relativePath);
		var resolved = new Uri(apiAddress, relativePath);
		return !resolved.AbsoluteUri.StartsWith(apiAddress.AbsoluteUri, StringComparison.Ordinal)
			? throw new ArgumentException(RootEscapeError, nameof(relativePath))
			: resolved;
	}
}
