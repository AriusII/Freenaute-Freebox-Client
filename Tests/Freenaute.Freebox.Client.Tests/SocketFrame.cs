namespace Freenaute.Freebox.Client.Tests;

internal sealed record SocketFrame(byte[] Bytes, WebSocketMessageType Type, bool EndOfMessage);
