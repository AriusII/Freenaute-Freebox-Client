namespace Freenaute.Freebox.Mapper.Contracts.Network;

/// <summary>Preserves the documented string/boolean discrepancy without interpreting a string as a boolean.</summary>
[JsonConverter(typeof(StringOrBooleanJsonConverter))]
public readonly struct StringOrBoolean
{
	private readonly string? _string;
	private readonly bool _boolean;

	private StringOrBoolean(string value)
	{
		Kind = StringOrBooleanKind.String;
		_string = value;
	}

	private StringOrBoolean(bool value)
	{
		Kind = StringOrBooleanKind.Boolean;
		_boolean = value;
	}

	public StringOrBooleanKind Kind { get; }

	public string String => Kind is StringOrBooleanKind.String
		? _string!
		: throw new InvalidOperationException("This value is not a string.");

	public bool Boolean => Kind is StringOrBooleanKind.Boolean
		? _boolean
		: throw new InvalidOperationException("This value is not a boolean.");

	public static StringOrBoolean FromString(string value) =>
		new(value ?? throw new ArgumentNullException(nameof(value)));

	public static StringOrBoolean FromBoolean(bool value) => new(value);

	public static implicit operator StringOrBoolean(string value) => FromString(value);

	public static implicit operator StringOrBoolean(bool value) => FromBoolean(value);
}
