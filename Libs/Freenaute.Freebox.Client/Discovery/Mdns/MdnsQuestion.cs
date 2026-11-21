namespace Freenaute.Freebox.Client.Discovery.Mdns;

internal readonly record struct MdnsQuestion(string Name, MdnsRecordType Type);
