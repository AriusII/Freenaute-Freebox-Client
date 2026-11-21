namespace Freenaute.Freebox.Mapper.Tests;

public sealed class FileSystemContractTests
{
	private static void AssertRoundTrip<T>(string wire, JsonTypeInfo<T> typeInfo) =>
		WireRoundTrip.AssertByteIdentical(wire, typeInfo);

	[Fact]
	public void FileEntry_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"path":"RGlzcXVlIGR1ci9WaWTDqW9z","name":"Videos","mimetype":"","type":"dir","size":0,"modification":1700000000,"index":0,"link":false,"hidden":false,"foldercount":2,"filecount":10,"exif":{"DateTimeOriginal":"2026-01-01"}}""",
			FilesJsonSerializerContext.Default.FileEntry);

	[Fact]
	public void FileListing_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"entries":[{"path":"RGlzcXVlIGR1cg==","name":"Disque dur","type":"dir","size":0,"hidden":false,"foldercount":2,"filecount":10}],"cursor":"abc"}""",
			FilesJsonSerializerContext.Default.FileListing);

	[Fact]
	public void FsTask_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":5,"type":"archive","state":"done","error":"","created_ts":1,"started_ts":2,"done_ts":3,"duration":1,"progress":100,"eta":0,"from":"/a","to":"/b","nfiles":2,"nfiles_done":2,"total_bytes":2048,"total_bytes_done":2048,"curr_bytes":2048,"curr_bytes_done":2048,"rate":1024,"src":["/a"],"dst":"/b"}""",
			FilesJsonSerializerContext.Default.FsTask);

	[Fact]
	public void FileHash_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"hash":"d41d8cd98f00b204e9800998ecf8427e"}""",
			FilesJsonSerializerContext.Default.FileHash);

	[Fact]
	public void OperationProgress_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"done_steps":3,"max_steps":10,"percent":30}""",
			FilesJsonSerializerContext.Default.OperationProgress);

	[Fact]
	public void FileUpload_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"id":1,"size":1024,"uploaded":512,"status":"in_progress","start_date":1700000000,"last_update":1700000100,"upload_name":"a.bin","dirname":"RGlzcXVlIGR1cg=="}""",
			FilesJsonSerializerContext.Default.FileUpload);

	[Fact]
	public void CreateDirectoryRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"parent":"RGlzcXVlIGR1cg==","dirname":"NewFolder"}""",
			FilesJsonSerializerContext.Default.CreateDirectoryRequest);

	[Fact]
	public void RenameFileRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"src":"L2E=","dst":"renamed.txt"}""",
			FilesJsonSerializerContext.Default.RenameFileRequest);

	[Fact]
	public void RemoveFilesRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"files":["L2E=","L2I="]}""",
			FilesJsonSerializerContext.Default.RemoveFilesRequest);

	[Fact]
	public void ArchiveFilesRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"files":["L2E="],"dst":"L2I="}""",
			FilesJsonSerializerContext.Default.ArchiveFilesRequest);

	[Fact]
	public void ExtractFilesRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"src":"L2E=","dst":"L2I=","delete_archive":true,"overwrite":false}""",
			FilesJsonSerializerContext.Default.ExtractFilesRequest);

	[Fact]
	public void ConcatenateFilesRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"files":["L2E=","L2I="],"dst":"Lw==","multi_volumes":true,"append":false}""",
			FilesJsonSerializerContext.Default.ConcatenateFilesRequest);

	[Fact]
	public void FileTransferRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"files":["L2E="],"dst":"Lw==","mode":"overwrite"}""",
			FilesJsonSerializerContext.Default.FileTransferRequest);

	[Fact]
	public void HashFileRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"src":"L2E=","hash_type":"md5"}""",
			FilesJsonSerializerContext.Default.HashFileRequest);

	[Fact]
	public void RepairFilesRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"src":"L2E=","delete_archive":true}""",
			FilesJsonSerializerContext.Default.RepairFilesRequest);

	[Fact]
	public void SetFileTaskStateRequest_RoundTripsByteIdentical() =>
		AssertRoundTrip(
			"""{"state":"paused"}""",
			FilesJsonSerializerContext.Default.SetFileTaskStateRequest);
}
