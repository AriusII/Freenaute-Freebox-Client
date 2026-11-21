namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnServerTypeJsonConverter : JsonConverter<VpnServerType>
{
    public override VpnServerType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnServerType(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnServerType.");
    }

    public override void Write(Utf8JsonWriter writer, VpnServerType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
