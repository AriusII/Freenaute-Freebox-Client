namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnClientTypeJsonConverter : JsonConverter<VpnClientType>
{
    public override VpnClientType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnClientType(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnClientType.");
    }

    public override void Write(Utf8JsonWriter writer, VpnClientType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
