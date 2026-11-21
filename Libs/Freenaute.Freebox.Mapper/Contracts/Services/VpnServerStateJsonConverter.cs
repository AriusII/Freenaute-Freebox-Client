namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class VpnServerStateJsonConverter : JsonConverter<VpnServerState>
{
    public override VpnServerState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new VpnServerState(reader.GetString()!)
            : throw new JsonException("Expected a string for VpnServerState.");
    }

    public override void Write(Utf8JsonWriter writer, VpnServerState value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
