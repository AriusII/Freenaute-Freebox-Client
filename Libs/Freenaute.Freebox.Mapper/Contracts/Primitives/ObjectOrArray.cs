namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>
///     A bounded JSON object-or-array union with a typed list view.
///     The list view does not change the received JSON shape.
/// </summary>
public readonly struct ObjectOrArray<T> : IEquatable<ObjectOrArray<T>>
{
	private readonly ReadOnlyCollection<T>? _items;

	private ObjectOrArray(ObjectOrArrayKind kind, T[] items)
	{
		Kind = kind;
		_items = Array.AsReadOnly(items);
	}

	public ObjectOrArrayKind Kind { get; }
	public bool IsInitialized => Kind is not ObjectOrArrayKind.Undefined;
	public bool IsObject => Kind is ObjectOrArrayKind.Object;
	public bool IsArray => Kind is ObjectOrArrayKind.Array;

	public T Object =>
		IsObject ? _items![0] : throw new InvalidOperationException("This JSON result is not an object.");

	/// <summary>Gets a read-only normalized view; undefined is distinct from an empty array.</summary>
	public IReadOnlyList<T> Items =>
		_items ?? throw new InvalidOperationException("This JSON result is not initialized.");

	public static ObjectOrArray<T> FromObject(T value)
	{
		return value is null
			? throw new ArgumentNullException(nameof(value))
			: new ObjectOrArray<T>(ObjectOrArrayKind.Object, [value]);
	}

	public static ObjectOrArray<T> FromArray(IEnumerable<T> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		var items = values.ToArray();
		if (items.Any(static item => item is null))
		{
			throw new ArgumentException("The typed object collection cannot contain null items.", nameof(values));
		}

		return new ObjectOrArray<T>(ObjectOrArrayKind.Array, items);
	}

	/// <summary>Requires exactly one item while retaining the original wire shape.</summary>
	public T GetSingle()
	{
		return Items.Count is 1
			? Items[0]
			: throw new InvalidOperationException("This JSON result does not contain exactly one item.");
	}

	public bool Equals(ObjectOrArray<T> other)
	{
		return Kind == other.Kind &&
		       (!IsInitialized || Items.SequenceEqual(other.Items));
	}

	public override bool Equals(object? obj) => obj is ObjectOrArray<T> other && Equals(other);

	public override int GetHashCode()
	{
		var hash = new HashCode();
		hash.Add(Kind);
		if (IsInitialized)
		{
			foreach (var item in Items)
			{
				hash.Add(item);
			}
		}

		return hash.ToHashCode();
	}

	public static bool operator ==(ObjectOrArray<T> left, ObjectOrArray<T> right) => left.Equals(right);

	public static bool operator !=(ObjectOrArray<T> left, ObjectOrArray<T> right) => !left.Equals(right);
}
