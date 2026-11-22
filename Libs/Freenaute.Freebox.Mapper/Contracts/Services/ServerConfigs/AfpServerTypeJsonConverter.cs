namespace Freenaute.Freebox.Mapper.Contracts.Services.ServerConfigs;

public sealed class AfpServerTypeJsonConverter : JsonConverter<AfpServerType>
{
	public override AfpServerType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.TokenType is JsonTokenType.String
			? new AfpServerType(reader.GetString()!)
			: throw new JsonException("Expected a string for AfpServerType.");
	}

	public override void Write(Utf8JsonWriter writer, AfpServerType value, JsonSerializerOptions options)
	{
		if (value.Value is null)
		{
			throw new JsonException(MapperConstants.ExceptionMessages.JsonConverters.UninitializedWireValueCannotBeSerialized);
		}

		writer.WriteStringValue(value.Value);
	}
}
