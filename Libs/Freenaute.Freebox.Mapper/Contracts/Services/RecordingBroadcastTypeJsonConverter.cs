namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class RecordingBroadcastTypeJsonConverter : JsonConverter<RecordingBroadcastType>
{
    public override RecordingBroadcastType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new RecordingBroadcastType(reader.GetString()!)
            : throw new JsonException("Expected a string for RecordingBroadcastType.");
    }

    public override void Write(Utf8JsonWriter writer, RecordingBroadcastType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
