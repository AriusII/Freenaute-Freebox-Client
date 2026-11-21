namespace Freenaute.Freebox.Mapper.Contracts.Network.Wifi;

/// <summary>The formal integer type and illustrated map of boolean radio capability flags are retained as separate kinds.</summary>
[JsonConverter(typeof(WifiBandCapabilitiesJsonConverter))]
public readonly struct WifiBandCapabilities
{
	private readonly long _integer;
	private readonly IReadOnlyDictionary<string, bool>? _flags;

	private WifiBandCapabilities(long value)
	{
		Kind = WifiBandCapabilitiesKind.Integer;
		_integer = value;
	}

	private WifiBandCapabilities(IReadOnlyDictionary<string, bool> flags)
	{
		Kind = WifiBandCapabilitiesKind.BooleanMap;
		_flags = flags;
	}

	public WifiBandCapabilitiesKind Kind { get; }

	public long Integer => Kind is WifiBandCapabilitiesKind.Integer
		? _integer
		: throw new InvalidOperationException("These capabilities are not an integer.");

	public IReadOnlyDictionary<string, bool> Flags => Kind is WifiBandCapabilitiesKind.BooleanMap
		? _flags!
		: throw new InvalidOperationException("These capabilities are not a boolean map.");

	public static WifiBandCapabilities FromInteger(long value) => new(value);

	public static WifiBandCapabilities FromFlags(IEnumerable<KeyValuePair<string, bool>> flags)
	{
		ArgumentNullException.ThrowIfNull(flags);
		return new WifiBandCapabilities(
			new ReadOnlyDictionary<string, bool>(new Dictionary<string, bool>(flags, StringComparer.Ordinal)));
	}
}
