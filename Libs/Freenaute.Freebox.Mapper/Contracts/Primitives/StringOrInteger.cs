namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>
///     Preserves the JSON kind for a documented string-or-integer contract.
///     A string such as "001" is never normalized to the integer 1.
/// </summary>
[JsonConverter(typeof(StringOrIntegerJsonConverter))]
public readonly struct StringOrInteger : IEquatable<StringOrInteger>
{
    private readonly string? _string;
    private readonly long _integer;

    private StringOrInteger(string value)
    {
        Kind = StringOrIntegerKind.String;
        _string = value;
    }

    private StringOrInteger(long value)
    {
        Kind = StringOrIntegerKind.Integer;
        _integer = value;
    }

    public StringOrIntegerKind Kind { get; }
    public bool IsInitialized => Kind != StringOrIntegerKind.Undefined;

    public string String => Kind == StringOrIntegerKind.String
        ? _string!
        : throw new InvalidOperationException("This JSON value is not a string.");

    public long Integer => Kind == StringOrIntegerKind.Integer
        ? _integer
        : throw new InvalidOperationException("This JSON value is not an integer.");

    public static StringOrInteger FromString(string value)
    {
        return new StringOrInteger(value ?? throw new ArgumentNullException(nameof(value)));
    }

    public static StringOrInteger FromInteger(long value)
    {
        return new StringOrInteger(value);
    }

    public static implicit operator StringOrInteger(string value)
    {
        return FromString(value);
    }

    public static implicit operator StringOrInteger(long value)
    {
        return FromInteger(value);
    }

    public static implicit operator StringOrInteger(int value)
    {
        return FromInteger(value);
    }

    public bool Equals(StringOrInteger other)
    {
        return Kind == other.Kind && Kind switch
        {
            StringOrIntegerKind.String => string.Equals(_string, other._string, StringComparison.Ordinal),
            StringOrIntegerKind.Integer => _integer == other._integer,
            _ => true
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is StringOrInteger other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Kind switch
        {
            StringOrIntegerKind.String => HashCode.Combine(Kind, _string),
            StringOrIntegerKind.Integer => HashCode.Combine(Kind, _integer),
            _ => 0
        };
    }

    public static bool operator ==(StringOrInteger left, StringOrInteger right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StringOrInteger left, StringOrInteger right)
    {
        return !left.Equals(right);
    }
}
