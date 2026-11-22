namespace Freenaute.Freebox.Client.Domains.Services;

internal static class ServicesPath
{
	private const string RelativeSegmentError = "A resource identifier cannot be a relative path segment.";

	internal static string Id(long id)
	{
		return id >= 0
			? id.ToString(CultureInfo.InvariantCulture)
			: throw new ArgumentOutOfRangeException(nameof(id));
	}

	internal static string Segment(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);
		if (value is "." or "..")
		{
			throw new ArgumentException(RelativeSegmentError, nameof(value));
		}

		return Uri.EscapeDataString(value);
	}
}
