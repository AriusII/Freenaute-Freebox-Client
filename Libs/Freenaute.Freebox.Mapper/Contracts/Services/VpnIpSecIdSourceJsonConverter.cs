namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnIpSecIdSourceJsonConverter : JsonConverter<VpnIpSecIdSource>
{
    public override VpnIpSecIdSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnIpSecIdSource(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnIpSecIdSource.");
    }

    public override void Write(Utf8JsonWriter writer, VpnIpSecIdSource value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
