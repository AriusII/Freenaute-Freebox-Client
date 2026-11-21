namespace Freenaute.Freebox.Mapper;

/// <summary>
///     Identifies the Freebox API version targeted by this mapper/contract library.
/// </summary>
public static class FreeboxApiVersion
{
	/// <summary>The major API version used in request paths.</summary>
	public const int Major = 4;

	/// <summary>The path segment representing the current API version.</summary>
	public const string Segment = "v4";

	/// <summary>The full advertised API version string.</summary>
	public const string Current = "4.0";
}
