namespace Freenaute.Freebox.Mapper.Tests;

public sealed class RoundTripSerializationTests
{
	[Fact]
	public void ApiVersionResponse_RoundTrips()
	{
		var original = new ApiVersionResponse(
			"uid",
			"device",
			"fbx",
			"Freebox",
			FreeboxApiVersion.Current,
			"example.com",
			"/api",
HttpsAvailable: true,
			443);

		string json = original.ToWire(FreeboxJsonSerializerContext.Default.ApiVersionResponse);
		var roundtrip = json.FromWire(FreeboxJsonSerializerContext.Default.ApiVersionResponse);

		Assert.NotNull(roundtrip);
		Assert.Equal(original.Uid, roundtrip!.Uid);
		Assert.Equal(original.ApiVersion, roundtrip.ApiVersion);
		Assert.Equal(original.ApiBaseUrl, roundtrip.ApiBaseUrl);
	}

	[Fact]
	public void StringOrInteger_RoundTrips()
	{
		var value = StringOrInteger.FromInteger(42);
		string json = value.ToWire(SharedJsonSerializerContext.Default.StringOrInteger);
		var roundtrip = json.FromWire(SharedJsonSerializerContext.Default.StringOrInteger);
		Assert.Equal(value, roundtrip);
	}

	[Fact]
	public void FreeboxMapper_UsesFrozenOptions()
	{
		var mapper = new FreeboxMapper();
		Assert.True(mapper.Options.IsReadOnly);
	}

	[Fact]
	public void MiscServerConfigPatches_PreserveExplicitFalseZeroAndWriteOnlyValues()
	{
		var ftp = new FtpConfigPatch { Enabled = false, PortCtrl = 0L, Password = "write-only-secret" };
		var samba = new SambaConfigPatch { Smbv2Enabled = false, LogonPassword = "logon-secret" };
		var afp = new AfpConfigPatch
		{
			Enabled = false,
			GuestAllow = false,
			ServerType = AfpServerType.Macmini,
			LoginPassword = "afp-secret"
		};
		var upnpAv = new UPnpAvConfigPatch { Enabled = false };
		var tftp = new TftpConfigPatch { Enabled = false, Root = "/mnt" };

		using var ftpWire = JsonDocument.Parse(ftp.ToWire(ServicesJsonSerializerContext.Default.FtpConfigPatch));
		Assert.False(ftpWire.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(0, ftpWire.RootElement.GetProperty("port_ctrl").GetInt64());
		Assert.Equal("write-only-secret", ftpWire.RootElement.GetProperty("password").GetString());
		using var sambaWire = JsonDocument.Parse(samba.ToWire(ServicesJsonSerializerContext.Default.SambaConfigPatch));
		Assert.False(sambaWire.RootElement.GetProperty("smbv2_enabled").GetBoolean());
		Assert.Equal("logon-secret", sambaWire.RootElement.GetProperty("logon_password").GetString());
		using var afpWire = JsonDocument.Parse(afp.ToWire(ServicesJsonSerializerContext.Default.AfpConfigPatch));
		Assert.False(afpWire.RootElement.GetProperty("enabled").GetBoolean());
		Assert.False(afpWire.RootElement.GetProperty("guest_allow").GetBoolean());
		Assert.Equal("macmini", afpWire.RootElement.GetProperty("server_type").GetString());
		Assert.Equal("afp-secret", afpWire.RootElement.GetProperty("login_password").GetString());
		Assert.Equal("{\"enabled\":false}",
			upnpAv.ToWire(ServicesJsonSerializerContext.Default.UPnpAvConfigPatch));
		Assert.Equal("{\"enabled\":false,\"root\":\"/mnt\"}",
			tftp.ToWire(ServicesJsonSerializerContext.Default.TftpConfigPatch));
	}

	[Fact]
	public void MiscServerConfigs_RoundTripReadAndWriteProjections()
	{
		string ftpJson =
			"""{"enabled":false,"allow_anonymous":true,"allow_anonymous_write":false,"username":"freebox","allow_remote_access":true,"weak_password":false,"port_ctrl":2121,"port_data":2122,"remote_domain":"freebox.test"}""";
		var ftp = ftpJson.FromWire(ServicesJsonSerializerContext.Default.FtpConfig);
		Assert.False(ftp?.Enabled);
		Assert.True(ftp?.AllowAnonymous);
		Assert.False(ftp?.AllowAnonymousWrite);
		Assert.Equal("freebox", ftp?.Username);
		Assert.Equal(2121, ftp?.PortCtrl);
		Assert.Equal("freebox.test", ftp?.RemoteDomain);

		string sambaJson =
			"""{"file_share_enabled":true,"print_share_enabled":false,"logon_enabled":true,"logon_user":"freebox","workgroup":"fixture","smbv2_enabled":false}""";
		var samba = sambaJson.FromWire(ServicesJsonSerializerContext.Default.SambaConfig);
		Assert.True(samba?.FileShareEnabled);
		Assert.False(samba?.PrintShareEnabled);
		Assert.False(samba?.Smbv2Enabled);

		string afpJson =
			"""{"enabled":true,"guest_allow":false,"server_type":"future-device","login_name":"freebox"}""";
		var afp = afpJson.FromWire(ServicesJsonSerializerContext.Default.AfpConfig);
		Assert.True(afp?.Enabled);
		Assert.False(afp?.GuestAllow);
		Assert.Equal("future-device", afp?.ServerType?.Value);

		var upnpAv = """{"enabled":false}"""
			.FromWire(ServicesJsonSerializerContext.Default.UPnpAvConfig);
		Assert.False(upnpAv?.Enabled);

		var tftp = """{"enabled":true,"root":"/srv/tftp"}"""
			.FromWire(ServicesJsonSerializerContext.Default.TftpConfig);
		Assert.True(tftp?.Enabled);
		Assert.Equal("/srv/tftp", tftp?.Root);
	}
}
