namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnIpProviderJsonConverter : JsonConverter<VpnIpProvider>
{
    public override VpnIpProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnIpProvider(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnIpProvider.");
    }

    public override void Write(Utf8JsonWriter writer, VpnIpProvider value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
