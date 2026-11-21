namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

public sealed class StringOrIntegerJsonConverter : JsonConverter<StringOrInteger>
{
    public override bool HandleNull => true;

    public override StringOrInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => StringOrInteger.FromString(reader.GetString()!),
            JsonTokenType.Number when reader.TryGetInt64(out long integer) => StringOrInteger.FromInteger(integer),
            _ => throw new JsonException("Expected a JSON string or a signed 64-bit integer.")
        };
    }

    public override void Write(Utf8JsonWriter writer, StringOrInteger value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case StringOrIntegerKind.String:
                writer.WriteStringValue(value.String);
                return;
            case StringOrIntegerKind.Integer:
                writer.WriteNumberValue(value.Integer);
                return;
            default:
                throw new JsonException("An uninitialized string-or-integer value cannot be written.");
        }
    }
}
