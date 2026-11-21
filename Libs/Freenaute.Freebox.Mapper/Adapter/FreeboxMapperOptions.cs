namespace Freenaute.Freebox.Mapper.Adapter;

/// <summary>
///     Frozen (read-only) <see cref="JsonSerializerOptions" /> instances shared by the mapper.
/// </summary>
public static class FreeboxMapperOptions
{
	/// <summary>A frozen options instance backed only by retained source-generated contexts.</summary>
	public static JsonSerializerOptions Default { get; } = CreateDefault();

	private static JsonSerializerOptions CreateDefault()
	{
		JsonSerializerContext shared = CreateContext(
			static options => new SharedJsonSerializerContext(options),
			new OptionalBooleanJsonConverter(),
			new OptionalInt32JsonConverter(),
			new OptionalInt64JsonConverter(),
			new OptionalStringJsonConverter(),
			new OptionalDecimalJsonConverter());
		var freebox = CreateContext(static options => new FreeboxJsonSerializerContext(options));
		JsonSerializerContext files = CreateContext(
			static options => new FilesJsonSerializerContext(options),
			new ObjectOrArrayJsonConverter<Download>());
		JsonSerializerContext network = CreateContext(
			static options => new NetworkJsonSerializerContext(options),
			new ObjectOrArrayJsonConverter<LanHostL2Ident>(),
			new ObjectOrArrayJsonConverter<WifiGlobalState>());
		var services = CreateContext(static options => new ServicesJsonSerializerContext(options));
		JsonSerializerContext protocol = CreateContext(
			static options => new ProtocolJsonSerializerContext(options),
			new NotificationTargetResultJsonConverter());
		var systemHome = CreateContext(static options => new SystemHomeJsonSerializerContext(options));
		var webSockets = CreateContext(static options => new WebSocketJsonSerializerContext(options));

		var options = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNameCaseInsensitive = true,
			TypeInfoResolver = JsonTypeInfoResolver.Combine(
				new ClosedConverterResolver(),
				shared,
				freebox,
				files,
				network,
				services,
				protocol,
				systemHome,
				webSockets)
		};
		options.MakeReadOnly();
		return options;
	}

	private static TContext CreateContext<TContext>(Func<JsonSerializerOptions, TContext> factory,
		params JsonConverter[] converters)
		where TContext : JsonSerializerContext
	{
		var options = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNameCaseInsensitive = true
		};
		foreach (var converter in converters)
		{
			options.Converters.Add(converter);
		}

		return factory(options);
	}

	private sealed class ClosedConverterResolver : IJsonTypeInfoResolver
	{
		private readonly ConcurrentDictionary<Type, JsonTypeInfo> _cache = new();

		public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			if (_cache.TryGetValue(type, out var cached))
			{
				return cached;
			}

			JsonTypeInfo? created = null;
			if (type == typeof(Optional<bool>))
			{
				created = JsonMetadataServices.CreateValueInfo<Optional<bool>>(options,
					new OptionalBooleanJsonConverter());
			}
			else if (type == typeof(Optional<int>))
			{
				created = JsonMetadataServices.CreateValueInfo<Optional<int>>(options,
					new OptionalInt32JsonConverter());
			}
			else if (type == typeof(Optional<long>))
			{
				created = JsonMetadataServices.CreateValueInfo<Optional<long>>(options,
					new OptionalInt64JsonConverter());
			}
			else if (type == typeof(Optional<string>))
			{
				created = JsonMetadataServices.CreateValueInfo<Optional<string>>(options,
					new OptionalStringJsonConverter());
			}
			else if (type == typeof(Optional<decimal>))
			{
				created = JsonMetadataServices.CreateValueInfo<Optional<decimal>>(options,
					new OptionalDecimalJsonConverter());
			}
			else if (type == typeof(ObjectOrArray<Download>))
			{
				created = JsonMetadataServices.CreateValueInfo<ObjectOrArray<Download>>(options,
					new ObjectOrArrayJsonConverter<Download>());
			}
			else if (type == typeof(ObjectOrArray<LanHostL2Ident>))
			{
				created = JsonMetadataServices.CreateValueInfo<ObjectOrArray<LanHostL2Ident>>(options,
					new ObjectOrArrayJsonConverter<LanHostL2Ident>());
			}
			else if (type == typeof(ObjectOrArray<WifiGlobalState>))
			{
				created = JsonMetadataServices.CreateValueInfo<ObjectOrArray<WifiGlobalState>>(options,
					new ObjectOrArrayJsonConverter<WifiGlobalState>());
			}
			else if (type == typeof(ObjectOrArray<NotificationTarget>))
			{
				created = JsonMetadataServices.CreateValueInfo<ObjectOrArray<NotificationTarget>>(options,
					new NotificationTargetResultJsonConverter());
			}

			return created is null
				? null
				: _cache.GetOrAdd(type, created);
		}
	}
}
