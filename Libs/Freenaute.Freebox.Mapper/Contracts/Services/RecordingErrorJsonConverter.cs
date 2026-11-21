namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class RecordingErrorJsonConverter : JsonConverter<RecordingError>
{
    public override RecordingError Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new RecordingError(reader.GetString()!)
            : throw new JsonException("Expected a string for RecordingError.");
    }

    public override void Write(Utf8JsonWriter writer, RecordingError value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
