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
			ContactId = 999,
			Number = "0999999999",
			IsDefault = false
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
		await volume.Level(0).Muted(mute: false).SendAsync();
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
	public async Task FormalCallLogItemRoutesUseTheDocumentedMethods()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			FixtureHandler.Json(request.Method == HttpMethod.Get &&
								request.Address.AbsolutePath.EndsWith("/call/log/", StringComparison.Ordinal)
				? """{"success":true,"result":[]}"""
				: """{"success":true,"result":{"id":69,"type":"missed","datetime":1359546363,"number":"0102030405","name":"Bob","duration":1,"new":false,"contact_id":56}}""")));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var call = new FreeboxServicesApi(client.Transport).Call;

		await call.GetLogAsync();
		await call.GetLogEntryAsync(69);
		await call.UpdateLogEntryAsync(69, new CallEntryPatch { New = false });
		await call.DeleteLogEntryAsync(69);

		var requests = handler.Requests
			.Where(r => r.Address.AbsolutePath.Contains("/call/log", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/call/log/", requests[0].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[0].Method);
		Assert.Equal("/api/v16/call/log/69", requests[1].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[1].Method);
		Assert.Equal("/api/v16/call/log/69", requests[2].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Put, requests[2].Method);
		Assert.Equal("{\"new\":false}", requests[2].Body);
		Assert.Equal("/api/v16/call/log/69", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Delete, requests[3].Method);
	}

	[Fact]
	public async Task CallLogRejectsNegativeIdentifiersBeforeHttp()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var call = new FreeboxServicesApi(client.Transport).Call;

		Assert.Throws<ArgumentOutOfRangeException>(() => call.LogEntry(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await call.GetLogEntryAsync(-1));
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task ContactsItemsAndInternalPvrRrdRoutesUseDocumentedPaths()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath switch
			{
				"/api/v16/rrd/" when request.Method == HttpMethod.Post =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"date_start":1,"date_end":2,"data":[{"rx":42}]}}"""),
				"/api/v16/rrd/" when request.Method == HttpMethod.Get =>
					FixtureHandler.Json(
						"""{"success":true,"result":{"date_start":1,"date_end":2,"data":[{"rx":42}]}}"""),
				"/api/v16/pvr/programmed/" =>
					FixtureHandler.Json("""{"success":true,"result":[]}"""),
				_ => FixtureHandler.Json("""{"success":true,"result":{"id":1}}""")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var services = new FreeboxServicesApi(client.Transport);

		await services.Contacts.GetItemsAsync(7, "numbers");
		await services.Pvr.GetConfigurationAsync();
		await services.Pvr.GetProgrammedAsync();
		await services.Rrd.GetAsync();
		var getStats = await services.Rrd.GetAsync(new RRDFetch
		{
			Db = "net",
			DateStart = 1,
			DateEnd = 2,
			Precision = 100,
			Fields = ["rx", "tx"]
		});
		var stats = await services.Rrd.FetchAsync(new RRDFetch
		{
			Db = "net",
			DateStart = 1,
			DateEnd = 2,
			Fields = ["rx"]
		});

		var requests = handler.Requests
			.Where(r => !r.Address.AbsolutePath.Contains("/login/", StringComparison.Ordinal)).ToArray();
		Assert.Equal("/api/v16/contact/7/numbers/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/pvr/config/", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/pvr/programmed/", requests[2].Address.AbsolutePath);
		Assert.Equal("/api/v16/rrd/", requests[3].Address.AbsolutePath);
		Assert.Equal(HttpMethod.Get, requests[4].Method);
		Assert.Equal("/api/v16/rrd/", requests[4].Address.AbsolutePath);
		Assert.Equal("?db=net&date_start=1&date_end=2&precision=100&fields=rx&fields=tx",
			requests[4].Address.Query);
		Assert.Equal(42, getStats.Data![0].GetProperty("rx").GetInt32());
		Assert.Equal(HttpMethod.Post, requests[5].Method);
		Assert.Equal("/api/v16/rrd/", requests[5].Address.AbsolutePath);
		Assert.Equal(1, stats.DateStart);
		Assert.Equal(2, stats.DateEnd);
		using var fetchBody = JsonDocument.Parse(requests[5].Body!);
		Assert.Equal("net", fetchBody.RootElement.GetProperty("db").GetString());
		Assert.Equal("rx", fetchBody.RootElement.GetProperty("fields")[0].GetString());
	}

	[Fact]
	public async Task ContactsPluralAliasesSupportAllDocumentedKinds()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":[{"id":1}]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var contacts = new FreeboxServicesApi(client.Transport).Contacts;

		await contacts.GetNumbersAsync(7);
		await contacts.GetAddressesAsync(7);
		await contacts.GetUrlsAsync(7);
		await contacts.GetEmailsAsync(7);

		string[] requests =
		[
			.. handler.Requests
				.Where(r => r.Address.AbsolutePath.Contains("/contact/7/", StringComparison.Ordinal))
				.Select(r => r.Address.AbsolutePath)
		];
		Assert.Equal(
			new[]
			{
				"/api/v16/contact/7/numbers/", "/api/v16/contact/7/addresses/", "/api/v16/contact/7/urls/",
				"/api/v16/contact/7/emails/"
			}, requests);
	}

	[Fact]
	public async Task ContactsSingularAliasesUseTopLevelItemRoutes()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":{"id":1}}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var contact = new FreeboxServicesApi(client.Transport).Contacts.Contact(7);

		await contact.Numbers.Item(1).GetAsync();
		await contact.Addresses.Item(2).GetAsync();
		await contact.Urls.Item(3).GetAsync();
		await contact.Emails.Item(4).GetAsync();

		string[] requests =
		[
			.. handler.Requests
				.Where(r => r.Address.AbsolutePath.Contains("/api/v16/", StringComparison.Ordinal))
				.Select(r => r.Address.AbsolutePath)
				.Where(path => path.Contains("/number/", StringComparison.Ordinal)
							   || path.Contains("/address/", StringComparison.Ordinal)
							   || path.Contains("/url/", StringComparison.Ordinal)
							   || path.Contains("/email/", StringComparison.Ordinal))
		];
		Assert.Equal(new[] { "/api/v16/number/1", "/api/v16/address/2", "/api/v16/url/3", "/api/v16/email/4" },
			requests);
	}

	[Fact]
	public async Task ContactsBracketStyleConnectorsExposeTopLevelNumberAddressUrlEmailRoutes()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(
			request.Address.AbsolutePath switch
			{
				"/api/v16/number/" => FixtureHandler.Json(
					"""{"success":true,"result":{"id":1,"contact_id":7,"number":"0601020304"}}"""),
				"/api/v16/address/" => FixtureHandler.Json(
					"""{"success":true,"result":{"id":2,"contact_id":7,"city":"Paris"}}"""),
				"/api/v16/url/" => FixtureHandler.Json(
					"""{"success":true,"result":{"id":3,"contact_id":7,"url":"https://x"}}"""),
				"/api/v16/email/" => FixtureHandler.Json(
					"""{"success":true,"result":{"id":4,"contact_id":7,"email":"a@b.c"}}"""),
				_ => FixtureHandler.Json("""{"success":true,"result":{"id":1,"contact_id":7}}""")
			}));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var contacts = new FreeboxServicesApi(client.Transport).Contacts;

		await contacts.CreateNumberAsync(new ContactNumberWriteRequest
		{
			ContactId = 7,
			Number = "0601020304",
			Type = ContactNumberType.Mobile
		});
		await contacts.Number(1).GetAsync();
		await contacts.CreateAddressAsync(new ContactAddressWriteRequest { ContactId = 7, City = "Paris" });
		await contacts.Address(2).GetAsync();
		await contacts.CreateUrlAsync(new ContactUrlWriteRequest { ContactId = 7, Url = "https://x" });
		await contacts.Url(3).GetAsync();
		await contacts.CreateEmailAsync(new ContactEmailWriteRequest { ContactId = 7, Email = "a@b.c" });
		await contacts.Email(4).GetAsync();

		string[] requests =
		[
			.. handler.Requests
				.Where(r => !r.Address.AbsolutePath.Contains("/login/", StringComparison.Ordinal))
				.Select(r => $"{r.Method} {r.Address.AbsolutePath}")
		];
		Assert.Equal(
			new[]
			{
				"POST /api/v16/number/", "GET /api/v16/number/1", "POST /api/v16/address/",
				"GET /api/v16/address/2", "POST /api/v16/url/", "GET /api/v16/url/3", "POST /api/v16/email/",
				"GET /api/v16/email/4"
			}, requests);
	}

	[Fact]
	public async Task ContactsListSupportsDocumentedQueryParameters()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":[]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var contacts = new FreeboxServicesApi(client.Transport).Contacts;

		await contacts.GetAllAsync(new ContactListOptions { Start = 0, Limit = -1, GroupId = 42 });

		var request =
			handler.Requests.Single(r => r.Address.AbsolutePath.Contains("/contact/", StringComparison.Ordinal));
		Assert.Equal("/api/v16/contact/", request.Address.AbsolutePath);
		Assert.Equal("?start=0&limit=-1&group_id=42", request.Address.Query);
	}

	[Fact]
	public async Task ContactsListRejectsOutOfRangeQueryParameters()
	{
		using var handler = FixtureHandler.AuthenticatedConstant("""{"success":true,"result":[]}""");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var contacts = new FreeboxServicesApi(client.Transport).Contacts;

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
		{
			await contacts.GetAllAsync(new ContactListOptions { Start = -1 });
		});
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
		{
			await contacts.GetAllAsync(new ContactListOptions { Limit = -2 });
		});
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
		{
			await contacts.GetAllAsync(new ContactListOptions { GroupId = -1 });
		});
		Assert.Empty(handler.Requests);
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
