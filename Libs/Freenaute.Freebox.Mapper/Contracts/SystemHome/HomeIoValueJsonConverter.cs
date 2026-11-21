namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

public sealed class HomeIoValueJsonConverter : JsonConverter<HomeIoValue>
{
    public override bool HandleNull => true;

    public override HomeIoValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null: return HomeIoValue.Null;
            case JsonTokenType.True: return HomeIoValue.FromBoolean(true);
            case JsonTokenType.False: return HomeIoValue.FromBoolean(false);
            case JsonTokenType.String: return HomeIoValue.FromString(reader.GetString()!);
            case JsonTokenType.Number:
                string number = reader.HasValueSequence
                    ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                    : Encoding.UTF8.GetString(reader.ValueSpan);
                if (!number.Contains('.') && !number.Contains('e') && !number.Contains('E'))
                {
                    if (!reader.TryGetInt64(out long integer))
                    {
                        throw new JsonException("The Home integer exceeds Int64.");
                    }

                    return HomeIoValue.FromInteger(integer);
                }

                return HomeIoValue.ReadFloat(reader.GetDouble(), number);
            default: throw new JsonException("Home values accept only null, Boolean, integer, float or string.");
        }
    }

    public override void Write(Utf8JsonWriter writer, HomeIoValue value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value.Kind)
        {
            case HomeIoValueKind.Null: writer.WriteNullValue(); break;
            case HomeIoValueKind.Boolean: writer.WriteBooleanValue(value.Boolean); break;
            case HomeIoValueKind.Integer: writer.WriteNumberValue(value.Integer); break;
            case HomeIoValueKind.Float: writer.WriteRawValue(value.NumberText!); break;
            case HomeIoValueKind.String: writer.WriteStringValue(value.String); break;
            default: throw new JsonException("Unsupported Home scalar kind.");
        }
    }
}
