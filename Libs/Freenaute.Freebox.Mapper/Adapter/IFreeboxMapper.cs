namespace Freenaute.Freebox.Mapper.Adapter;

/// <summary>
///     Adapter contract for converting between client-facing DTOs and Freebox wire contracts.
/// </summary>
internal interface IFreeboxMapper
{
	/// <summary>The frozen serializer options used by this mapper.</summary>
	JsonSerializerOptions Options { get; }

	/// <summary>Resolves generated metadata for a registered wire type.</summary>
	JsonTypeInfo<T> GetTypeInfo<T>()
	{
		var type = typeof(T);
		try
		{
			return (JsonTypeInfo<T>)Options.GetTypeInfo(type);
		}
		catch (NotSupportedException exception)
		{
			string name = type.FullName ?? type.Name;
			throw new NotSupportedException(
				string.Format(
					CultureInfo.InvariantCulture,
					MapperConstants.ExceptionMessages.Adapter.TypeNotRegisteredInSourceGeneratedContextsFormat,
					name),
				exception);
		}
	}

	/// <summary>Serializes a registered value to its wire JSON representation.</summary>
	string ToWire<T>(T value) => ToWire(value, GetTypeInfo<T>());

	/// <summary>Serializes a registered value to its UTF-8 wire JSON representation.</summary>
	byte[] ToUtf8Bytes<T>(T value) => JsonSerializer.SerializeToUtf8Bytes(value, GetTypeInfo<T>());

	/// <summary>Serializes a value with explicitly supplied generated metadata.</summary>
	string ToWire<T>(T value, JsonTypeInfo<T> typeInfo);

	/// <summary>Serializes a value with explicitly supplied generated metadata.</summary>
	byte[] ToUtf8Bytes<T>(T value, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.SerializeToUtf8Bytes(value, typeInfo);
	}

	/// <summary>Deserializes a registered wire JSON representation.</summary>
	T? FromWire<T>(string json) => FromWire(json, GetTypeInfo<T>());

	/// <summary>Deserializes a registered UTF-8 wire JSON representation.</summary>
	T? FromWire<T>(ReadOnlySpan<byte> json) => JsonSerializer.Deserialize(json, GetTypeInfo<T>());

	/// <summary>Deserializes a registered JSON element.</summary>
	T? FromWire<T>(JsonElement json) => json.Deserialize(GetTypeInfo<T>());

	/// <summary>Deserializes a wire JSON representation with explicitly supplied generated metadata.</summary>
	T? FromWire<T>(string json, JsonTypeInfo<T> typeInfo);

	/// <summary>Deserializes a UTF-8 wire JSON representation with explicitly supplied generated metadata.</summary>
	T? FromWire<T>(ReadOnlySpan<byte> json, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.Deserialize(json, typeInfo);
	}

	/// <summary>Deserializes a JSON element with explicitly supplied generated metadata.</summary>
	T? FromWire<T>(JsonElement json, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return json.Deserialize(typeInfo);
	}
}
