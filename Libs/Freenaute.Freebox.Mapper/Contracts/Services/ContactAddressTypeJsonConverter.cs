namespace Freenaute.Freebox.Mapper.Contracts.Services;

public sealed class ContactAddressTypeJsonConverter : JsonConverter<ContactAddressType>
{
    public override ContactAddressType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new ContactAddressType(reader.GetString()!)
            : throw new JsonException("Expected a string for ContactAddressType.");
    }

    public override void Write(Utf8JsonWriter writer, ContactAddressType value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            throw new JsonException("An uninitialized wire value cannot be serialized.");
        }

        writer.WriteStringValue(value.Value);
    }
}
