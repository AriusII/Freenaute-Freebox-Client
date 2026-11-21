namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class RecordingChannelTypeJsonConverter : JsonConverter<RecordingChannelType>
{
    public override RecordingChannelType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new RecordingChannelType(reader.GetString()!)
            : throw new JsonException("Expected a string for RecordingChannelType.");
    }

    public override void Write(Utf8JsonWriter writer, RecordingChannelType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
