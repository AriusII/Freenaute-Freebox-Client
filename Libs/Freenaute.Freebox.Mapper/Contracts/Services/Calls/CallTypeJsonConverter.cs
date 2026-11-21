namespace Freenaute.Freebox.Mapper.Contracts.Services.Calls;

public sealed class CallTypeJsonConverter : JsonConverter<CallType>
{
	public override CallType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new CallType(reader.GetString()!)
			: throw new JsonException("Expected a string for CallType.");
	}

	public override void Write(Utf8JsonWriter writer, CallType value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException("An uninitialized wire value cannot be serialized.");
		}

		writer.WriteStringValue(value.Value);
	}
}
