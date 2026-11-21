namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class PvrRecordTimeJsonConverter : JsonConverter<PvrRecordTime>
{
    public override PvrRecordTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => PvrRecordTime.FromInteger(reader.GetInt64()),
            JsonTokenType.StartObject => PvrRecordTime.FromMap(
                JsonSerializer.Deserialize(ref reader, ServicesJsonSerializerContext.Default.RecordingTimeMap)!),
            _ => throw new JsonException("Expected an integer or a per-source recording-time map.")
        };
    }

    public override void Write(Utf8JsonWriter writer, PvrRecordTime value, JsonSerializerOptions options)
    {
        switch (value.Kind)
        {
            case PvrRecordTimeKind.Integer: writer.WriteNumberValue(value.Integer); break;
            case PvrRecordTimeKind.Map:
                JsonSerializer.Serialize(writer, value.WireMap,
                    ServicesJsonSerializerContext.Default.RecordingTimeMap); break;
            default: throw new JsonException("An uninitialized recording time cannot be serialized.");
        }
    }
}
