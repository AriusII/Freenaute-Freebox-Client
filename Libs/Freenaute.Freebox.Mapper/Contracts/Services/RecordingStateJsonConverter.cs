namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class RecordingStateJsonConverter : JsonConverter<RecordingState>
{
    public override RecordingState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new RecordingState(reader.GetString()!)
            : throw new JsonException("Expected a string for RecordingState.");
    }

    public override void Write(Utf8JsonWriter writer, RecordingState value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
