namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

public sealed class EmptyObjectOrInt32ArrayJsonConverter : JsonConverter<EmptyObjectOrInt32Array>
{
    public override bool HandleNull => true;

    public override EmptyObjectOrInt32Array Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("Only an empty object is documented for this integer-array union.");
            }

            return EmptyObjectOrInt32Array.EmptyObject;
        }

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected an empty JSON object or an integer array.");
        }

        List<int> values = [];
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return EmptyObjectOrInt32Array.FromArray(values);
            }

            if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int value))
            {
                throw new JsonException("Expected a signed 32-bit integer array item.");
            }

            values.Add(value);
        }

        throw new JsonException("The integer array was not terminated.");
    }

    public override void Write(Utf8JsonWriter writer, EmptyObjectOrInt32Array value, JsonSerializerOptions options)
    {
        if (!value.IsInitialized)
        {
            throw new JsonException("An uninitialized empty-object-or-integer-array value cannot be written.");
        }

        if (value.Kind == EmptyObjectOrInt32ArrayKind.EmptyObject)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
            return;
        }

        writer.WriteStartArray();
        foreach (int item in value.Values)
        {
            writer.WriteNumberValue(item);
        }

        writer.WriteEndArray();
    }
}
