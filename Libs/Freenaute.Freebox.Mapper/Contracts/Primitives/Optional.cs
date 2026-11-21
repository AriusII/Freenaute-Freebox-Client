namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>
///     Represents an omitted field, an explicit JSON null, or a supplied value.
///     Patch properties must use <c>JsonIgnoreCondition.WhenWritingDefault</c> to omit unset values.
///     Explicit null is appropriate only when the operation documents that request value.
/// </summary>
public readonly struct Optional<T> : IEquatable<Optional<T>>
{
    private readonly T? _value;

    private Optional(OptionalValueKind kind, T? value)
    {
        Kind = kind;
        _value = value;
    }

    public OptionalValueKind Kind { get; }
    public bool IsSet => Kind != OptionalValueKind.Unset;
    public bool HasValue => Kind == OptionalValueKind.Value;
    public bool IsNull => Kind == OptionalValueKind.Null;
    public static Optional<T> Unset => default;
    public static Optional<T> Null => new(OptionalValueKind.Null, default);

    /// <summary>Gets the supplied non-null value; rejects omitted and explicitly null fields.</summary>
    public T Value => HasValue
        ? _value!
        : throw new InvalidOperationException("The optional field does not contain a value.");

    public static Optional<T> FromValue(T value)
    {
        return value is null ? Null : new Optional<T>(OptionalValueKind.Value, value);
    }

    public static Optional<T> Set(T value)
    {
        return FromValue(value);
    }

    public static implicit operator Optional<T>(T value)
    {
        return FromValue(value);
    }

    /// <summary>Returns the supplied value or default; use Kind when absence and null matter.</summary>
    [return: MaybeNull]
    public T GetValueOrDefault()
    {
        return HasValue ? _value! : default!;
    }

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = HasValue ? _value! : default!;
        return HasValue;
    }

    public bool Equals(Optional<T> other)
    {
        return Kind == other.Kind &&
               (!HasValue || EqualityComparer<T>.Default.Equals(_value!, other._value!));
    }

    public override bool Equals(object? obj)
    {
        return obj is Optional<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HasValue ? HashCode.Combine(Kind, _value) : Kind.GetHashCode();
    }

    public static bool operator ==(Optional<T> left, Optional<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Optional<T> left, Optional<T> right)
    {
        return !left.Equals(right);
    }
}
