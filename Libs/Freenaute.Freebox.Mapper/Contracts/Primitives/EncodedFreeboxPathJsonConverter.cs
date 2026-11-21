namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

public sealed class EncodedFreeboxPathJsonConverter : JsonConverter<EncodedFreeboxPath>
{
	public override bool HandleNull => true;

	public override EncodedFreeboxPath Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		if (reader.TokenType is not JsonTokenType.String)
		{
			throw new JsonException("Expected a Base64-encoded Freebox path string.");
		}

		try
		{
			return EncodedFreeboxPath.FromEncoded(reader.GetString()!);
		}
		catch (FormatException exception)
		{
			throw new JsonException("The Freebox path has invalid Base64 syntax.", exception);
		}
	}

	public override void Write(Utf8JsonWriter writer, EncodedFreeboxPath value, JsonSerializerOptions options)
	{
		if (!value.IsInitialized)
		{
			throw new JsonException("An uninitialized Freebox path cannot be written.");
		}

		writer.WriteStringValue(value.Value);
	}
}
