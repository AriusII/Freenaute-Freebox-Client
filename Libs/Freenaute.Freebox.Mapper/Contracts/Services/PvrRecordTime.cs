namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Preserves the integer declaration or the per-source/per-quality map in the official response example.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#Media.record_time" />
[JsonConverter(typeof(PvrRecordTimeJsonConverter))]
public readonly struct PvrRecordTime
{
    private readonly long _integer;
    private readonly Dictionary<string, Dictionary<string, long>>? _map;

    private PvrRecordTime(long value)
    {
        Kind = PvrRecordTimeKind.Integer;
        _integer = value;
        _map = null;
    }

    private PvrRecordTime(Dictionary<string, Dictionary<string, long>> value)
    {
        Kind = PvrRecordTimeKind.Map;
        _integer = default;
        _map = value;
    }

    public PvrRecordTimeKind Kind { get; }

    public long Integer => Kind == PvrRecordTimeKind.Integer
        ? _integer
        : throw new InvalidOperationException("The recording time is not an integer.");

    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, long>> Map => Kind == PvrRecordTimeKind.Map
        ? _map!.ToDictionary(p => p.Key,
            p => (IReadOnlyDictionary<string, long>)new ReadOnlyDictionary<string, long>(p.Value),
            StringComparer.Ordinal)
        : throw new InvalidOperationException("The recording time is not a map.");

    public static PvrRecordTime FromInteger(long value)
    {
        return new PvrRecordTime(value);
    }

    public static PvrRecordTime FromMap(Dictionary<string, Dictionary<string, long>> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new PvrRecordTime(value.ToDictionary(p => p.Key,
            p => new Dictionary<string, long>(p.Value, StringComparer.Ordinal), StringComparer.Ordinal));
    }

    internal Dictionary<string, Dictionary<string, long>> WireMap => _map!;

    public override string ToString()
    {
        return nameof(PvrRecordTime);
    }
}
