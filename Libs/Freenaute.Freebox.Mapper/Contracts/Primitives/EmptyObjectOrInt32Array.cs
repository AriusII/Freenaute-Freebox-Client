namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>A documented empty object or an integer array; a nonempty object is never accepted as an arbitrary map.</summary>
[JsonConverter(typeof(EmptyObjectOrInt32ArrayJsonConverter))]
public readonly struct EmptyObjectOrInt32Array : IEquatable<EmptyObjectOrInt32Array>
{
    private readonly ReadOnlyCollection<int>? _values;

    private EmptyObjectOrInt32Array(EmptyObjectOrInt32ArrayKind kind, int[] values)
    {
        Kind = kind;
        _values = Array.AsReadOnly(values);
    }

    public EmptyObjectOrInt32ArrayKind Kind { get; }
    public bool IsInitialized => Kind != EmptyObjectOrInt32ArrayKind.Undefined;

    public IReadOnlyList<int> Values =>
        _values ?? throw new InvalidOperationException("This integer collection is not initialized.");

    public static EmptyObjectOrInt32Array EmptyObject => new(EmptyObjectOrInt32ArrayKind.EmptyObject, []);

    public static EmptyObjectOrInt32Array FromArray(IEnumerable<int> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        return new EmptyObjectOrInt32Array(EmptyObjectOrInt32ArrayKind.Array, values.ToArray());
    }

    public bool Equals(EmptyObjectOrInt32Array other)
    {
        return Kind == other.Kind &&
               (!IsInitialized || Values.SequenceEqual(other.Values));
    }

    public override bool Equals(object? obj)
    {
        return obj is EmptyObjectOrInt32Array other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Kind);
        if (IsInitialized)
        {
            foreach (int value in Values)
            {
                hash.Add(value);
            }
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(EmptyObjectOrInt32Array left, EmptyObjectOrInt32Array right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EmptyObjectOrInt32Array left, EmptyObjectOrInt32Array right)
    {
        return !left.Equals(right);
    }
}
