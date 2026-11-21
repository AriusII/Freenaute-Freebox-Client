namespace Freenaute.Freebox.Client;

internal readonly record struct MdnsQuestion(string Name, MdnsRecordType Type);
