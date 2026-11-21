namespace Freenaute.Freebox.Mapper.Tests;

public sealed class ParentalContractTests
{
	private readonly FreeboxMapper _mapper = new();

	[Fact]
	public void ParentalFilterConfig_RoundTrips()
	{
		var original = new ParentalFilterConfig { DefaultFilterMode = ParentalFilterState.Denied };
		string json = original.ToWire(ServicesJsonSerializerContext.Default.ParentalFilterConfig);

		Assert.Equal("""{"default_filter_mode":"denied"}""", json);
		var roundtrip = json.FromWire(ServicesJsonSerializerContext.Default.ParentalFilterConfig);

		Assert.NotNull(roundtrip);
		Assert.Equal(ParentalFilterState.Denied, roundtrip!.DefaultFilterMode);
	}

	[Fact]
	public void ParentalFilterConfigPatch_OmitsUnsetAndRejectsExplicitNull()
	{
		Assert.Equal("{}", _mapper.ToWire(new ParentalFilterConfigPatch()));
		Assert.Equal("""{"default_filter_mode":"webonly"}""",
			_mapper.ToWire(new ParentalFilterConfigPatch { DefaultFilterMode = ParentalFilterState.Webonly }));
		Assert.Throws<ArgumentException>(() =>
			new ParentalFilterConfigPatch { DefaultFilterMode = Optional<ParentalFilterState>.Null }.Validate());
	}

	[Fact]
	public void ParentalFilter_RoundTripsDocumentedExample()
	{
		const string wire = """
		                    {
		                      "forced": false,
		                      "id": 4,
		                      "hosts": ["iPad-r0ro", "iPhone-r0ro"],
		                      "tmp_mode_expire": 0,
		                      "ip": "0.0.0.0",
		                      "forced_mode": "allowed",
		                      "scheduling_mode": "planning",
		                      "macs": ["D8:A2:CA:FE:BA:DF", "D0:23:BE:DE:AD:EF"],
		                      "current_mapping_idx": 225,
		                      "next_change": 620,
		                      "desc": "r0ro",
		                      "tmp_mode": "allowed",
		                      "filter_state": "allowed"
		                    }
		                    """;

		var filter = wire.FromWire(ServicesJsonSerializerContext.Default.ParentalFilter);

		Assert.NotNull(filter);
		Assert.Equal(4, filter!.Id);
		Assert.Equal(["D8:A2:CA:FE:BA:DF", "D0:23:BE:DE:AD:EF"], filter.Macs!);
		Assert.Equal(["iPad-r0ro", "iPhone-r0ro"], filter.Hosts!);
		Assert.Equal("0.0.0.0", filter.Ip);
		Assert.Equal("r0ro", filter.Desc);
		Assert.False(filter.Forced);
		Assert.Equal(ParentalFilterState.Allowed, filter.ForcedMode);
		Assert.Equal(0, filter.TmpModeExpire);
		Assert.Equal(ParentalFilterState.Allowed, filter.TmpMode);
		Assert.Equal(ParentalSchedulingMode.Planning, filter.SchedulingMode);
		Assert.Equal(ParentalFilterState.Allowed, filter.FilterState);
		Assert.Equal(225, filter.CurrentMappingIdx);
		Assert.Equal(620, filter.NextChange);

		string reserialized = filter.ToWire(ServicesJsonSerializerContext.Default.ParentalFilter);
		var roundtrip = reserialized.FromWire(ServicesJsonSerializerContext.Default.ParentalFilter);

		Assert.NotNull(roundtrip);
		Assert.Equal(filter.Id, roundtrip!.Id);
		Assert.Equal(filter.Macs, roundtrip.Macs);
		Assert.Equal(filter.Hosts, roundtrip.Hosts);
		Assert.Equal(filter.FilterState, roundtrip.FilterState);
		Assert.Equal(filter.SchedulingMode, roundtrip.SchedulingMode);
		Assert.Equal(filter.NextChange, roundtrip.NextChange);
	}

	[Fact]
	public void ParentalFilterWriteRequest_SerializesOnlySetFields()
	{
		Assert.Equal("{}", _mapper.ToWire(new ParentalFilterWriteRequest()));

		var request = new ParentalFilterWriteRequest
		{
			Macs = new[] { "00:07:cb:00:00:69", "00:07:cb:00:00:42" },
			Forced = true,
			ForcedMode = ParentalFilterState.Denied,
			TmpModeExpire = 0
		};
		string json = _mapper.ToWire(request);

		using var doc = JsonDocument.Parse(json);
		Assert.Equal(4, doc.RootElement.EnumerateObject().Count());
		Assert.Equal("00:07:cb:00:00:69", doc.RootElement.GetProperty("macs")[0].GetString());
		Assert.True(doc.RootElement.GetProperty("forced").GetBoolean());
		Assert.Equal("denied", doc.RootElement.GetProperty("forced_mode").GetString());
		Assert.Equal(0, doc.RootElement.GetProperty("tmp_mode_expire").GetInt64());
	}

	[Fact]
	public void ParentalFilterPlanning_RoundTrips()
	{
		var original = new ParentalFilterPlanning
		{
			Resolution = 48,
			Cdayranges = [":fr_bank_holidays,:fr_school_holidays_c,"],
			Mapping = [ParentalFilterState.Allowed, ParentalFilterState.Webonly, ParentalFilterState.Denied]
		};
		string json = original.ToWire(ServicesJsonSerializerContext.Default.ParentalFilterPlanning);

		Assert.Equal(
			"""{"resolution":48,"cdayranges":[":fr_bank_holidays,:fr_school_holidays_c,"],"mapping":["allowed","webonly","denied"]}""",
			json);
		var roundtrip = json.FromWire(ServicesJsonSerializerContext.Default.ParentalFilterPlanning);

		Assert.NotNull(roundtrip);
		Assert.Equal(48, roundtrip!.Resolution);
		Assert.Equal([":fr_bank_holidays,:fr_school_holidays_c,"], roundtrip.Cdayranges!);
		Assert.Equal([ParentalFilterState.Allowed, ParentalFilterState.Webonly, ParentalFilterState.Denied],
			roundtrip.Mapping!);
	}

	[Fact]
	public void ParentalFilterPlanningPatch_OmitsUnsetAndSerializesMapping()
	{
		Assert.Equal("{}", _mapper.ToWire(new ParentalFilterPlanningPatch()));

		var patch = new ParentalFilterPlanningPatch
		{
			Cdayranges = Array.Empty<string>(),
			Mapping = new[] { ParentalFilterState.Webonly, ParentalFilterState.Allowed }
		};
		string json = _mapper.ToWire(patch);

		Assert.Equal("""{"cdayranges":[],"mapping":["webonly","allowed"]}""", json);
		Assert.Throws<ArgumentException>(() =>
			new ParentalFilterPlanningPatch { Mapping = Optional<ParentalFilterState[]>.Null }.Validate());
	}

	[Theory]
	[InlineData("allowed", "allowed")]
	[InlineData("denied", "denied")]
	[InlineData("webonly", "webonly")]
	[InlineData("future_value", "future_value")]
	public void ParentalFilterState_PreservesUnknownWireValues(string wire, string expected)
	{
		var state = _mapper.FromWire<ParentalFilterState>($"\"{wire}\"");

		Assert.Equal(expected, state.Value);
		Assert.Equal($"\"{wire}\"", _mapper.ToWire(state));
	}

	[Theory]
	[InlineData("forced")]
	[InlineData("temporary")]
	[InlineData("planning")]
	public void ParentalSchedulingMode_RoundTrips(string wire)
	{
		var mode = _mapper.FromWire<ParentalSchedulingMode>($"\"{wire}\"");

		Assert.Equal(wire, mode.Value);
		Assert.Equal($"\"{wire}\"", _mapper.ToWire(mode));
	}
}
