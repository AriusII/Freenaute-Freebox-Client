namespace Freenaute.Freebox.Mapper.Tests;

public sealed class SystemHomeContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void SystemConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"firmware_version":"4.8.0","mac":"00:11:22:33:44:55","serial":"ABC123","uptime":"1 day","uptime_val":86400,"board_name":"fbx","box_authenticated":true,"disk_status":"active","usb3_enable":true,"user_main_storage":"disk","user_storage_powered":true,"sensors":[{"id":"sensor0","name":"temp_cpu","value":42}],"model_info":{"internal_hdd_size":1000000000000,"wifi_type":"wifi6","name":"Freebox Delta","pretty_name":"Freebox Delta","has_expansions":false,"has_lan_sfp":true,"has_dect":false,"has_home_automation":true,"has_femtocell_exp":false,"has_fixed_femtocell":false,"has_vm":false,"has_dsl":false,"has_standby":true,"has_eco_wifi":true,"has_wop":false,"has_led_strip":true,"has_status_led":true,"has_usb3_enable":true,"has_lcd_screensaver":true},"fans":[{"id":"fan0","name":"fan","value":2000}],"expansions":[{"slot":0,"probe_done":true,"present":true,"supported":true,"bundle":"","type":"security"}]}""",
			SystemHomeJsonSerializerContext.Default.SystemConfig);

	[Fact]
	public void LcdConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"brightness":80,"orientation_forced":true,"orientation":0,"hide_wifi_key":false,"led_strip_enabled":true,"led_strip_brightness":50,"led_strip_animation":"rainbow","available_led_strip_animations":["rainbow","static"],"hide_status_led":false,"screensaver":"night","hide_led":false}""",
			SystemHomeJsonSerializerContext.Default.LcdConfig);

	[Fact]
	public void StandbyConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"use_planning":true,"planning_mode":"standby","resolution":96,"mapping":[true,false,true]}""",
			SystemHomeJsonSerializerContext.Default.StandbyConfig);

	[Fact]
	public void StandbyStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"use_planning":false,"planning_mode":"wifi_off","next_change":1700000000,"available_planning_modes":["wifi_off","standby"]}""",
			SystemHomeJsonSerializerContext.Default.StandbyStatus);

	[Fact]
	public void Profile_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"name":"Maison","icon":"home","url":""}""",
			SystemHomeJsonSerializerContext.Default.Profile);

	[Fact]
	public void ProfileCreateRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"name":"Maison","url":""}""",
			SystemHomeJsonSerializerContext.Default.ProfileCreateRequest);

	[Fact]
	public void ProfileCreated_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":7}""",
			SystemHomeJsonSerializerContext.Default.ProfileCreated);

	[Fact]
	public void NetworkControl_RoundTripsByteIdentical_WithHostsNameUnion() =>
		AssertRoundTrip(
			"""{"profile_id":1,"next_change":1700000000,"override_mode":"allowed","current_mode":"webonly","rule_mode":"denied","override_until":1700003600,"override":true,"macs":["aa:bb:cc:dd:ee:ff"],"hosts":["freebox","phone"],"resolution":96,"cdayranges":["mon 00:00-08:00"]}""",
			SystemHomeJsonSerializerContext.Default.NetworkControl);

	[Fact]
	public void NetworkControlRule_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"profile_id":1,"name":"rule","mode":"denied","start_time":0,"end_time":3600,"weekdays":[true,true,true,true,true,false,false],"enabled":true}""",
			SystemHomeJsonSerializerContext.Default.NetworkControlRule);

	[Fact]
	public void NetworkControlUpdate_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"override_mode":"allowed","override_until":1700003600,"override":false,"macs":["aa:bb:cc:dd:ee:ff"],"cdayranges":["mon 00:00-08:00"]}""",
			SystemHomeJsonSerializerContext.Default.NetworkControlUpdate);

	[Fact]
	public void UpdateStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"state":"up_to_date","upgrade_state":{"state":"commit","old_version":"4.7.0","new_version":"4.8.0","percent":100,"error_string":""}}""",
			SystemHomeJsonSerializerContext.Default.UpdateStatus);

	[Fact]
	public void LanguageSupport_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"lang":"fr","avalaible":["fr","en"]}""",
			SystemHomeJsonSerializerContext.Default.LanguageSupport);

	[Fact]
	public void SetLanguageRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"lang":"en"}""",
			SystemHomeJsonSerializerContext.Default.SetLanguageRequest);

	[Fact]
	public void DefaultModeMigrationStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"default_mode_migrated":true}""",
			SystemHomeJsonSerializerContext.Default.DefaultModeMigrationStatus);

	[Fact]
	public void LedstripPlanning_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"use_planning":true,"planning_mode":"ledstrip_off","resolution":96,"mapping":[true,false]}""",
			SystemHomeJsonSerializerContext.Default.LedstripPlanning);

	[Fact]
	public void LedstripStatus_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"use_planning":true,"next_change":1700000000}""",
			SystemHomeJsonSerializerContext.Default.LedstripStatus);
}
