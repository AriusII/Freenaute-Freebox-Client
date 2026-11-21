namespace Freenaute.Freebox.Client.Domains.WebSockets;

public sealed record FreeboxUploadProgress(
    long BytesSent,
    long? TotalFileLength,
    long? ExpectedFileLength,
    bool Complete);
