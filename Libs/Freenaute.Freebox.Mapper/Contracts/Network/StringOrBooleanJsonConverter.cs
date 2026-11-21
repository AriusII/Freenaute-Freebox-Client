namespace Freenaute.Freebox.Mapper.Contracts.Network;

public sealed class StringOrBooleanJsonConverter : JsonConverter<StringOrBoolean>
{
    public override bool HandleNull => true;

    public override StringOrBoolean Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => StringOrBoolean.FromString(reader.GetString()!),
            JsonTokenType.True => StringOrBoolean.FromBoolean(true),
            JsonTokenType.False => StringOrBoolean.FromBoolean(false),
            _ => throw new JsonException("Expected a JSON string or boolean.")
        };
    }

    public override void Write(Utf8JsonWriter writer, StringOrBoolean value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case StringOrBooleanKind.String:
                writer.WriteStringValue(value.String);
                return;
            case StringOrBooleanKind.Boolean:
                writer.WriteBooleanValue(value.Boolean);
                return;
            default: throw new JsonException("An uninitialized string-or-boolean value cannot be written.");
        }
    }
}
