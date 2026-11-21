namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

public sealed class ParentalSchedulingModeJsonConverter : JsonConverter<ParentalSchedulingMode>
{
	public override ParentalSchedulingMode Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new ParentalSchedulingMode(reader.GetString()!)
			: throw new JsonException("Expected a string for ParentalSchedulingMode.");
	}

	public override void Write(Utf8JsonWriter writer, ParentalSchedulingMode value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException("An uninitialized wire value cannot be serialized.");
		}

		writer.WriteStringValue(value.Value);
	}
}
