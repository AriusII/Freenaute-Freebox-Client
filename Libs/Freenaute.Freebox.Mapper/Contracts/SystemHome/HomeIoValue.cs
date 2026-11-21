namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>A Home scalar retaining its documented JSON representation, including explicit null.</summary>
[JsonConverter(typeof(HomeIoValueJsonConverter))]
public sealed class HomeIoValue
{
    private readonly bool boolean;
    private readonly double floating;
    private readonly long integer;
    private readonly string? text;

    private HomeIoValue(HomeIoValueKind kind, bool boolean = false, long integer = 0,
        double floating = 0, string? text = null, string? numberText = null)
    {
        Kind = kind;
        this.boolean = boolean;
        this.integer = integer;
        this.floating = floating;
        this.text = text;
        NumberText = numberText;
    }

    internal string? NumberText { get; }

    public HomeIoValueKind Kind { get; }
    public static HomeIoValue Null { get; } = new(HomeIoValueKind.Null);
    public bool Boolean => Kind == HomeIoValueKind.Boolean ? boolean : throw WrongKind();
    public long Integer => Kind == HomeIoValueKind.Integer ? integer : throw WrongKind();
    public double Float => Kind == HomeIoValueKind.Float ? floating : throw WrongKind();
    public string String => Kind == HomeIoValueKind.String ? text! : throw WrongKind();

    public static HomeIoValue FromBoolean(bool value)
    {
        return new HomeIoValue(HomeIoValueKind.Boolean, value);
    }

    public static HomeIoValue FromInteger(long value)
    {
        return new HomeIoValue(HomeIoValueKind.Integer, integer: value);
    }

    public static HomeIoValue FromString(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HomeIoValue(HomeIoValueKind.String, text: value);
    }

    public static HomeIoValue FromFloat(double value)
    {
        if (!double.IsFinite(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        string number = value.ToString("R", CultureInfo.InvariantCulture);
        if (!number.Contains('.') && !number.Contains('E') && !number.Contains('e'))
        {
            number += ".0";
        }

        return new HomeIoValue(HomeIoValueKind.Float, floating: value, numberText: number);
    }

    internal static HomeIoValue ReadFloat(double value, string lexeme)
    {
        if (!double.IsFinite(value))
        {
            throw new JsonException("A Home float must be finite.");
        }

        return new HomeIoValue(HomeIoValueKind.Float, floating: value, numberText: lexeme);
    }

    private static InvalidOperationException WrongKind()
    {
        return new InvalidOperationException("The Home scalar has a different JSON kind.");
    }
}
