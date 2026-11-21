namespace Freenaute.Freebox.Client;

/// <summary>An authenticated operation requires configured credentials or an open session.</summary>
public sealed class FreeboxAuthenticationException(string message) : InvalidOperationException(message);
