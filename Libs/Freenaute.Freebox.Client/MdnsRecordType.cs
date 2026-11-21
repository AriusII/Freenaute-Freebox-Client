namespace Freenaute.Freebox.Client;

internal enum MdnsRecordType : ushort
{
	Address = 1,
	Pointer = 12,
	Text = 16,
	AddressV6 = 28,
	Service = 33
}
