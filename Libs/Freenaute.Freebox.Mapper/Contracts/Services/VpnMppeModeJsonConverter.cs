namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnMppeModeJsonConverter : JsonConverter<VpnMppeMode>
{
    public override VpnMppeMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnMppeMode(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnMppeMode.");
    }

    public override void Write(Utf8JsonWriter writer, VpnMppeMode value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
