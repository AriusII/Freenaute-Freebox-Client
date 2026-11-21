namespace Freenaute.Freebox.Mapper.Tests;

public sealed class ConverterRoundTripTests
{
	private readonly FreeboxMapper _mapper = new();

	[Fact]
	public void OptionalValues_PreserveNullFalseZeroAnd64BitBoundaries()
	{
		Assert.Equal("null", _mapper.ToWire(Optional<bool>.Null));
		Assert.Equal("false", _mapper.ToWire(Optional<bool>.FromValue(false)));
		Assert.Equal("0", _mapper.ToWire(Optional<int>.FromValue(0)));
		Assert.Equal("-9223372036854775808", _mapper.ToWire(Optional<long>.FromValue(long.MinValue)));
		Assert.Equal("9223372036854775807", _mapper.ToWire(Optional<long>.FromValue(long.MaxValue)));
	}

	[Fact]
	public void OptionalProperty_OmitsUnsetButRejectsSerializationOfUnsetStandaloneValue()
	{
		Assert.Equal("{}", _mapper.ToWire(new CallEntryPatch()));
		Assert.Equal("{\"new\":false}", _mapper.ToWire(new CallEntryPatch { New = false }));
		Assert.Throws<JsonException>(() => _mapper.ToWire(Optional<bool>.Unset));
	}

	[Theory]
	[InlineData("42", StringOrIntegerKind.Integer)]
	[InlineData("-9223372036854775808", StringOrIntegerKind.Integer)]
	[InlineData("\"001\"", StringOrIntegerKind.String)]
	public void StringOrInteger_RoundTripsWithoutNormalizing(string json, StringOrIntegerKind expectedKind)
	{
		var value = _mapper.FromWire<StringOrInteger>(json);

		Assert.Equal(expectedKind, value.Kind);
		Assert.Equal(json, _mapper.ToWire(value));
	}

	[Theory]
	[InlineData("false")]
	[InlineData("\"false\"")]
	public void StringOrBoolean_RoundTripsWithoutCoercion(string json)
	{
		var value = _mapper.FromWire<StringOrBoolean>(json);

		Assert.True(value.Kind is StringOrBooleanKind.Boolean or StringOrBooleanKind.String);
		Assert.Equal(json, _mapper.ToWire(value));
	}

	[Fact]
	public void EncodedPath_PreservesEncodedBytesAndRejectsInvalidWireValues()
	{
		var value = EncodedFreeboxPath.FromUtf8Path("Disque 1/été");

		Assert.Equal(value, _mapper.FromWire<EncodedFreeboxPath>(_mapper.ToWire(value)));
		Assert.Throws<JsonException>(() => _mapper.FromWire<EncodedFreeboxPath>("\"not base64!\""));
		Assert.Throws<JsonException>(() => _mapper.ToWire(default(EncodedFreeboxPath)));
	}

	[Theory]
	[InlineData("{\"id\":7}", false)]
	[InlineData("{\"id\":[7,8]}", true)]
	public void AddedDownloads_PreservesScalarAndArrayShape(string json, bool wasMultiple)
	{
		var value = _mapper.FromWire<AddedDownloads>(json)!;

		Assert.Equal(wasMultiple, value.WasMultiple);
		Assert.Equal(json, _mapper.ToWire(value));
	}

	[Fact]
	public void ObjectOrArray_PreservesObjectArrayAndEmptyArrayShapes()
	{
		var single = _mapper.FromWire<ObjectOrArray<Download>>("{\"id\":1}");
		var many = _mapper.FromWire<ObjectOrArray<Download>>("[{\"id\":1},{\"id\":2}]");
		var empty = _mapper.FromWire<ObjectOrArray<Download>>("[]");

		Assert.True(single.IsObject);
		Assert.Equal(2, many.Items.Count);
		Assert.True(empty.IsArray);
		Assert.Empty(empty.Items);
		Assert.Equal("{\"id\":1}", _mapper.ToWire(single));
		Assert.Equal("[]", _mapper.ToWire(empty));
	}

	[Fact]
	public void WifiCapabilities_PreservesIntegerAndBooleanMapShapes()
	{
		var integer = _mapper.FromWire<WifiBandCapabilities>("7");
		var flags = _mapper.FromWire<WifiBandCapabilities>("{\"ax\":true,\"he\":false}");

		Assert.Equal(WifiBandCapabilitiesKind.Integer, integer.Kind);
		Assert.Equal(WifiBandCapabilitiesKind.BooleanMap, flags.Kind);
		Assert.False(flags.Flags["he"]);
		Assert.Throws<JsonException>(() => _mapper.FromWire<WifiBandCapabilities>("{\"ax\":1}"));
	}

	[Fact]
	public void ProtocolNotificationUnion_RoundTripsObjectAndArray()
	{
		var single = _mapper.FromWire<ObjectOrArray<NotificationTarget>>("{\"id\":\"one\"}");
		var many = _mapper.FromWire<ObjectOrArray<NotificationTarget>>("[{\"id\":\"one\"},{\"id\":\"two\"}]");

		Assert.True(single.IsObject);
		Assert.Equal(2, many.Items.Count);
		var roundTrip = _mapper.FromWire<ObjectOrArray<NotificationTarget>>(_mapper.ToWire(single));
		Assert.True(roundTrip.IsObject);
		Assert.Equal("one", roundTrip.Object.Id);
	}

	[Fact]
	public void SystemHomeHostUnion_PreservesNamesAndRejectsMixedKinds()
	{
		var names = _mapper.FromWire<NetworkControlHosts>("[\"freebox\",\"phone\"]")!;

		Assert.Equal(NetworkControlHostsKind.Names, names.Kind);
		Assert.Equal("[\"freebox\",\"phone\"]", _mapper.ToWire(names));
		Assert.Throws<JsonException>(() => _mapper.FromWire<NetworkControlHosts>("[\"freebox\",{}]"));
	}

	[Fact]
	public void WireTokenConverters_PreserveUnknownStringsAndRejectNonStrings()
	{
		Assert.Equal("future", _mapper.FromWire<AfpServerType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<CallType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<ContactAddressType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<ContactEmailType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<ContactNumberType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<ContactUrlType>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<PlayerMediaCommandName>("\"future\"").Value);
		Assert.Equal("future", _mapper.FromWire<SystemDiskStatus>("\"future\"").Value);
		Assert.Throws<JsonException>(() => _mapper.FromWire<CallType>("1"));
	}

	[Theory]
	[InlineData("9223372036854775808")]
	[InlineData("1.5")]
	[InlineData("true")]
	[InlineData("null")]
	[InlineData("{\"id\":1}")]
	[InlineData("42 trailing")]
	public void StringOrInteger_RejectsInvalidOrUnboundedWireValues(string json) =>
		Assert.Throws<JsonException>(() => _mapper.FromWire<StringOrInteger>(json));

	[Fact]
	public void InvalidUtf8_IsRejected()
	{
		byte[] invalid = [0x22, 0x80, 0x22];

		Assert.Throws<JsonException>(() => _mapper.FromWire<StringOrInteger>(invalid));
	}
}
