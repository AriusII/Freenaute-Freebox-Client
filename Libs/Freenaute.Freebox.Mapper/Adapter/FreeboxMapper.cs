namespace Freenaute.Freebox.Mapper.Adapter;

/// <summary>Default source-gen/AOT-friendly implementation of <see cref="IFreeboxMapper" />.</summary>
public sealed class FreeboxMapper : IFreeboxMapper
{
	private readonly ConcurrentDictionary<Type, JsonTypeInfo> _typeInfoCache = new();

	/// <summary>A process-wide mapper backed by the frozen default resolver chain.</summary>
	public static FreeboxMapper Default { get; } = new();

	public JsonSerializerOptions Options => FreeboxMapperOptions.Default;

	public JsonTypeInfo<T> GetTypeInfo<T>()
	{
		var type = typeof(T);
		if (_typeInfoCache.TryGetValue(type, out var cached))
		{
			return (JsonTypeInfo<T>)cached;
		}

		JsonTypeInfo<T> resolved;
		try
		{
			resolved = (JsonTypeInfo<T>)Options.GetTypeInfo(type);
		}
		catch (NotSupportedException exception)
		{
			string name = type.FullName ?? type.Name;
			throw new NotSupportedException(
				$"The type '{name}' is not registered in the Freebox source-generated JSON contexts.", exception);
		}

		return (JsonTypeInfo<T>)_typeInfoCache.GetOrAdd(type, resolved);
	}

	public string ToWire<T>(T value, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.Serialize(value, typeInfo);
	}

	public T? FromWire<T>(string json, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(json);
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.Deserialize(json, typeInfo);
	}

	public string ToWire<T>(T value) => ToWire(value, GetTypeInfo<T>());

	public byte[] ToUtf8Bytes<T>(T value) => ToUtf8Bytes(value, GetTypeInfo<T>());

	public byte[] ToUtf8Bytes<T>(T value, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.SerializeToUtf8Bytes(value, typeInfo);
	}

	public T? FromWire<T>(string json) => FromWire(json, GetTypeInfo<T>());

	public T? FromWire<T>(ReadOnlySpan<byte> json) => FromWire(json, GetTypeInfo<T>());

	public T? FromWire<T>(JsonElement json) => FromWire(json, GetTypeInfo<T>());

	public T? FromWire<T>(ReadOnlySpan<byte> json, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return JsonSerializer.Deserialize(json, typeInfo);
	}

	public T? FromWire<T>(JsonElement json, JsonTypeInfo<T> typeInfo)
	{
		ArgumentNullException.ThrowIfNull(typeInfo);
		return json.Deserialize(typeInfo);
	}
}
