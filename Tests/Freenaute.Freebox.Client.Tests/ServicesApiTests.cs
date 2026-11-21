namespace Freenaute.Freebox.Client.Tests;

public sealed class ServicesApiTests
{
	[Fact]
	public async Task FluentConfigurationBranchesKeepExplicitFalseAndZeroWithoutSendingUnsetFields()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"enabled":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);
		var baseline = services.Ftp.Configure();
		var disabled = baseline.With(patch => patch with { Enabled = false, PortCtrl = 0 });
		var anonymous = baseline.With(patch => patch with { AllowAnonymous = false });
		Assert.Empty(handler.Requests);

		await disabled.SendAsync();
		await anonymous.SendAsync();

		var requests = handler.Requests.Where(r => r.Address.AbsolutePath.Contains("/ftp/", StringComparison.Ordinal))
			.ToArray();
		Assert.Equal("/api/v16/ftp/config/", requests[0].Address.AbsolutePath);
		using var first = JsonDocument.Parse(requests[0].Body!);
		using var second = JsonDocument.Parse(requests[1].Body!);
		Assert.False(first.RootElement.GetProperty("enabled").GetBoolean());
		Assert.Equal(0, first.RootElement.GetProperty("port_ctrl").GetInt64());
		Assert.Equal(2, first.RootElement.EnumerateObject().Count());
		Assert.False(second.RootElement.GetProperty("allow_anonymous").GetBoolean());
		Assert.Single(second.RootElement.EnumerateObject());
	}

	[Fact]
	public async Task ContactItemUsesGlobalRouteAndBindsSelectedContactWhenCreated()
	{
		using var handler = FixtureHandler.AuthenticatedConstant(
			"""{"success":true,"result":{"id":2,"contact_id":7,"number":"0999999999","is_default":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);
		var numbers = services.Contacts.Contact(7).Numbers;

		await numbers.CreateAsync(new ContactNumberWriteRequest
		{
			ContactId = 999, Number = "0999999999", IsDefault = false
		});
		await numbers.Item(2).UpdateAsync(new ContactNumberWriteRequest { IsOwn = false });

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/number/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/number/", requests[0].Address.AbsolutePath);
		using var first = JsonDocument.Parse(requests[0].Body!);
		Assert.Equal(7, first.RootElement.GetProperty("contact_id").GetInt64());
		Assert.False(first.RootElement.GetProperty("is_default").GetBoolean());
		Assert.Equal("/api/v16/number/2", requests[1].Address.AbsolutePath);
		Assert.Equal("{\"is_own\":false}", requests[1].Body);
	}

	[Fact]
	public async Task PlayerVolumePreservesInnerVersionAndIndependentCommands()
	{
		using var handler =
			FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"volume":0,"mute":false}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);
		var volume = services.Player.Device(3).Volume();
		await volume.Level(0).Muted(false).SendAsync();
		await volume.Level(50).SendAsync();

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/player/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/player/3/api/v6/control/volume/", requests[0].Address.AbsolutePath);
		using var first = JsonDocument.Parse(requests[0].Body!);
		Assert.Equal(0, first.RootElement.GetProperty("volume").GetInt32());
		Assert.False(first.RootElement.GetProperty("mute").GetBoolean());
		Assert.Equal("{\"volume\":50}", requests[1].Body);
	}

	[Fact]
	public async Task FormalCallBatchMethodsAndCurrentTftpRouteAreUsed()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);
		await services.Call.MarkAllLogEntriesAsReadAsync();
		await services.Call.DeleteAllLogEntriesAsync();
		await services.Tftp.UpdateConfigurationAsync(new TftpConfigPatch { Enabled = false });
		var requests = handler.Requests
			.Where(r => !r.Address.AbsolutePath.Contains("/login/", StringComparison.Ordinal)).ToArray();
		Assert.Equal(HttpMethod.Post, requests[0].Method);
		Assert.Equal(HttpMethod.Post, requests[1].Method);
		Assert.Equal("/api/v16/tftp/config/", requests[2].Address.AbsolutePath);
		Assert.Equal("{\"enabled\":false}", requests[2].Body);
	}

	[Fact]
	public async Task BinaryVoicemailKeepsEncodedIdentifiersAndSendsOnce()
	{
		using var handler = FixtureHandler.Authenticated((_, _) => Task.FromResult(
			new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent("RIFF-fixture", Encoding.ASCII, "audio/wav")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		await using var audio = await new FreeboxServicesApi(client.Transport, client.BinaryTransport).Call
			.Voicemail("20221215_154135_#1.au").DownloadAudioAsync();
		using var reader = new StreamReader(audio.Content, leaveOpen: true);
		Assert.Equal("RIFF-fixture", await reader.ReadToEndAsync());
		Assert.Equal("audio/wav", audio.ContentType);
		Assert.Equal("https://fixture.example/api/v16/call/voicemail/20221215_154135_%231.au/audio_file",
			handler.Requests.Last().Address.AbsoluteUri);
	}

	[Fact]
	public void ProjectionKeepsNumericKindsAndLargeCounters()
	{
		var voicemail = JsonSerializer.Deserialize("""{"id":"fixture.au","country_code":33} """,
			ServicesJsonSerializerContext.Default.VoicemailEntry)!;
		Assert.Equal(StringOrIntegerKind.Integer, voicemail.CountryCode!.Value.Kind);
	}

	[Fact]
	public void UnsupportedNullOrMissingSchemaAndInvalidVolumeFailBeforeHttp()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);
		Assert.Throws<ArgumentException>(() =>
		{
			_ = services.Ftp.UpdateConfigurationAsync(new FtpConfigPatch { Password = Optional<string>.Null });
		});
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_ = services.Player.Device(1).Volume().Level(101).SendAsync();
		});
		Assert.Throws<ArgumentException>(() =>
		{
			_ = services.Player.Device(1).SendMediaCommandAsync(new PlayerMediaCommandName("seek_to"));
		});
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public void RequestAndResponseToStringDoNotEmitSecretsOrContactDetails()
	{
		Assert.Equal(nameof(FtpConfigPatch), new FtpConfigPatch { Password = "fixture-secret" }.ToString());
		Assert.Equal(nameof(ContactWriteRequest),
			new ContactWriteRequest { DisplayName = "fixture-person" }.ToString());
	}
}
