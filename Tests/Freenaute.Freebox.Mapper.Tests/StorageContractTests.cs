namespace Freenaute.Freebox.Mapper.Tests;

public sealed class StorageContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void StorageConfig_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"external_pm_enabled":true,"external_pm_idle_before_spindown":10}""",
			FilesJsonSerializerContext.Default.StorageConfig);

	[Fact]
	public void StorageDisk_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"type":"internal","state":"enabled","connector":0,"total_bytes":250059350016,"table_type":"gpt","model":"Hitachi","serial":"GSCH35VC","firmware":"PB2ICC0E","temp":40,"operation_pct":{"done_steps":1,"max_steps":1,"percent":100},"partitions":[{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}],"idle":true,"idle_duration":0,"spinning":true,"active_duration":0,"time_before_spindown":232}""",
			FilesJsonSerializerContext.Default.StorageDisk);

	[Fact]
	public void DiskPartition_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}""",
			FilesJsonSerializerContext.Default.DiskPartition);

	[Fact]
	public void DiskPartition_PathUsesEncodedFreeboxPath()
	{
		var partition = JsonSerializer.Deserialize(
			"""{"id":3,"disk_id":1,"state":"mounted","fstype":"ext4","label":"Disque dur","path":"L0Rpc3F1ZSBkdXI=","total_bytes":245091500032,"used_bytes":164520534016,"free_bytes":68120969216,"fsck_result":"no_run_yet","operation_pct":{"done_steps":1,"max_steps":1,"percent":100}}""",
			FilesJsonSerializerContext.Default.DiskPartition)!;

		Assert.Equal("/Disque dur", partition.Path!.Value.DecodeUtf8Path());
	}

	[Fact]
	public void SetStorageDiskStateRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip("""{"state":"disabled"}""", FilesJsonSerializerContext.Default.SetStorageDiskStateRequest);

	[Fact]
	public void FormatStorageDiskRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"table_type":"gpt","fs_type":"ext4","label":"freebox"}""",
			FilesJsonSerializerContext.Default.FormatStorageDiskRequest);

	[Fact]
	public void SetStoragePartitionStateRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"state":"umounted"}""",
			FilesJsonSerializerContext.Default.SetStoragePartitionStateRequest);

	[Fact]
	public void CheckStoragePartitionRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"checkmode":"ro"}""",
			FilesJsonSerializerContext.Default.CheckStoragePartitionRequest);
}
