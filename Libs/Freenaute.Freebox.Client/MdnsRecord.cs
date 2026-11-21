namespace Freenaute.Freebox.Client;

internal sealed record MdnsRecord(
	string Name,
	MdnsRecordType Type,
	uint TimeToLive,
	string? Target,
	ushort? Port,
	byte[]? Text,
	IPAddress? Address);
