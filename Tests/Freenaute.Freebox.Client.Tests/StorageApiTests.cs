namespace Freenaute.Freebox.Client.Tests;

public sealed class StorageApiTests
{
	[Fact]
	public async Task StorageRoutesAndPayloadsMatchOpenApi()
	{
		using var handler = FixtureHandler.Authenticated((request, _) => Task.FromResult(FixtureHandler.Json(
			(request.Method.Method, request.Address.AbsolutePath) switch
			{
				("GET", "/api/v16/storage/config/") =>
					"""{"success":true,"result":{"external_pm_enabled":true,"external_pm_idle_before_spindown":10}}""",
				("PUT", "/api/v16/storage/config/") =>
					"""{"success":true,"result":{"external_pm_enabled":false,"external_pm_idle_before_spindown":15}}""",
				("GET", "/api/v16/storage/disk/") =>
					"""{"success":true,"result":[{"id":1,"type":"internal","state":"enabled","connector":0,"total_bytes":250059350016,"table_type":"gpt","model":"Hitachi","serial":"GSCH35VC","firmware":"PB2ICC0E","temp":40,"operation_pct":{"done_steps":1,"max_steps":1,"percent":100},"partitions":[{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}],"idle":true,"idle_duration":0,"spinning":true,"active_duration":0,"time_before_spindown":232}]}""",
				("GET", "/api/v16/storage/disk/1") =>
					"""{"success":true,"result":{"id":1,"type":"internal","state":"enabled","connector":0,"total_bytes":250059350016,"table_type":"gpt","model":"Hitachi","serial":"GSCH35VC","firmware":"PB2ICC0E","temp":40,"operation_pct":{"done_steps":1,"max_steps":1,"percent":100},"partitions":[],"idle":true,"idle_duration":0,"spinning":true,"active_duration":0,"time_before_spindown":232}}""",
				("PUT", "/api/v16/storage/disk/1") =>
					"""{"success":true,"result":{"id":1,"type":"internal","state":"disabled","connector":0,"total_bytes":250059350016,"table_type":"gpt","model":"Hitachi","serial":"GSCH35VC","firmware":"PB2ICC0E","temp":40,"operation_pct":{"done_steps":0,"max_steps":1,"percent":0},"partitions":[],"idle":false,"idle_duration":0,"spinning":false,"active_duration":0,"time_before_spindown":0}}""",
				("PUT", "/api/v16/storage/disk/1/format/") => """{"success":true}""",
				("GET", "/api/v16/storage/partition/") =>
					"""{"success":true,"result":[{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}]}""",
				("GET", "/api/v16/storage/partition/3") =>
					"""{"success":true,"result":{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}}""",
				("PUT", "/api/v16/storage/partition/3") =>
					"""{"success":true,"result":{"id":3,"disk_id":1,"state":"umounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":0,"max_steps":1,"percent":0}}}""",
				("PUT", "/api/v16/storage/partition/3/check/") => """{"success":true}""",
				_ => """{"success":true}"""
			})));
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var storage = new FreeboxNetworkApi(client.Transport).Storage;

		await storage.GetConfigurationAsync();
		await storage.UpdateConfigurationAsync(new StorageConfig
		{
			ExternalPowerManagementEnabled = false,
			ExternalIdleBeforeSpindownMinutes = 15
		});
		await storage.GetDisksAsync();
		await storage.GetDiskAsync(1);
		await storage.SetDiskStateAsync(1, new SetStorageDiskStateRequest { State = "disabled" });
		await storage.FormatDiskAsync(1,
			new FormatStorageDiskRequest { TableType = "gpt", FsType = "ext4", Label = "freebox" });
		await storage.GetPartitionsAsync();
		await storage.GetPartitionAsync(3);
		await storage.SetPartitionStateAsync(3, new SetStoragePartitionStateRequest { State = "umounted" });
		await storage.CheckPartitionAsync(3, new CheckStoragePartitionRequest { CheckMode = "ro" });

		var requests = handler.Requests.Where(request =>
			request.Address.AbsolutePath is not "/api_version" &&
			!request.Address.AbsolutePath.EndsWith("/login/", StringComparison.Ordinal) &&
			!request.Address.AbsolutePath.EndsWith("/login/session/", StringComparison.Ordinal)).ToArray();

		Assert.Equal(10, requests.Length);
		Assert.Equal("/api/v16/storage/config/", requests[0].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/config/", requests[1].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/", requests[2].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/1", requests[3].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/1", requests[4].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/disk/1/format/", requests[5].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/partition/", requests[6].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/partition/3", requests[7].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/partition/3", requests[8].Address.AbsolutePath);
		Assert.Equal("/api/v16/storage/partition/3/check/", requests[9].Address.AbsolutePath);

		using var configPutBody = JsonDocument.Parse(requests[1].Body!);
		Assert.False(configPutBody.RootElement.GetProperty("external_pm_enabled").GetBoolean());
		Assert.Equal(15, configPutBody.RootElement.GetProperty("external_pm_idle_before_spindown").GetInt64());

		using var diskStatePutBody = JsonDocument.Parse(requests[4].Body!);
		Assert.Equal("disabled", diskStatePutBody.RootElement.GetProperty("state").GetString());
		Assert.Single(diskStatePutBody.RootElement.EnumerateObject());

		using var formatPutBody = JsonDocument.Parse(requests[5].Body!);
		Assert.Equal("gpt", formatPutBody.RootElement.GetProperty("table_type").GetString());
		Assert.Equal("ext4", formatPutBody.RootElement.GetProperty("fs_type").GetString());
		Assert.Equal("freebox", formatPutBody.RootElement.GetProperty("label").GetString());

		using var partitionStatePutBody = JsonDocument.Parse(requests[8].Body!);
		Assert.Equal("umounted", partitionStatePutBody.RootElement.GetProperty("state").GetString());
		Assert.Single(partitionStatePutBody.RootElement.EnumerateObject());

		using var checkPutBody = JsonDocument.Parse(requests[9].Body!);
		Assert.Equal("ro", checkPutBody.RootElement.GetProperty("checkmode").GetString());
		Assert.Single(checkPutBody.RootElement.EnumerateObject());
	}

	[Fact]
	public async Task StorageWriteRoutesRejectNullAndEmptyInputsBeforeHttpRequest()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var storage = new FreeboxNetworkApi(client.Transport).Storage;

		await Assert.ThrowsAsync<ArgumentNullException>(async () => await storage.SetDiskStateAsync(1, null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await storage.FormatDiskAsync(1, null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await storage.SetPartitionStateAsync(1, null!));
		await Assert.ThrowsAsync<ArgumentNullException>(async () => await storage.CheckPartitionAsync(1, null!));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await storage.SetDiskStateAsync(1, new SetStorageDiskStateRequest { State = " " }));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await storage.FormatDiskAsync(1,
				new FormatStorageDiskRequest { TableType = " ", FsType = "ext4", Label = "freebox" }));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await storage.SetPartitionStateAsync(1, new SetStoragePartitionStateRequest { State = "" }));
		await Assert.ThrowsAsync<ArgumentException>(async () =>
			await storage.CheckPartitionAsync(1, new CheckStoragePartitionRequest { CheckMode = "" }));
		Assert.Empty(handler.Requests);
	}

	[Fact]
	public async Task StorageRoutesRejectNegativeIdsAndUnsupportedEnumValuesBeforeHttp()
	{
		using var handler = FixtureHandler.Constant("{}");
		using var http = new HttpClient(handler);
		using var client = HttpClientTests.CreateClient(http);
		var storage = new FreeboxNetworkApi(client.Transport).Storage;

		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await storage.GetDiskAsync(-1));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await storage.SetDiskStateAsync(-1,
			new SetStorageDiskStateRequest { State = "enabled" }));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await storage.FormatDiskAsync(-1,
			new FormatStorageDiskRequest { TableType = "gpt", FsType = "ext4", Label = "freebox" }));
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await storage.GetPartitionAsync(-1));

		await Assert.ThrowsAsync<ArgumentException>(async () => await storage.SetDiskStateAsync(1,
			new SetStorageDiskStateRequest { State = "unexpected" }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await storage.FormatDiskAsync(1,
			new FormatStorageDiskRequest { TableType = "unexpected", FsType = "ext4", Label = "freebox" }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await storage.FormatDiskAsync(1,
			new FormatStorageDiskRequest { TableType = "gpt", FsType = "unexpected", Label = "freebox" }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await storage.SetPartitionStateAsync(1,
			new SetStoragePartitionStateRequest { State = "unexpected" }));
		await Assert.ThrowsAsync<ArgumentException>(async () => await storage.CheckPartitionAsync(1,
			new CheckStoragePartitionRequest { CheckMode = "unexpected" }));
		Assert.Empty(handler.Requests);
	}
}
