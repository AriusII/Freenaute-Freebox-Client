namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnCipherJsonConverter : JsonConverter<VpnCipher>
{
    public override VpnCipher Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnCipher(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnCipher.");
    }

    public override void Write(Utf8JsonWriter writer, VpnCipher value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
