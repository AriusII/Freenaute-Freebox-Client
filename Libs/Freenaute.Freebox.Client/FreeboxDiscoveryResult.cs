namespace Freenaute.Freebox.Client;

/// <summary>Represents one mDNS/DNS-SD candidate and its advertised Freebox metadata.</summary>
/// <param name="Origin">The HTTPS origin advertised for subsequent Freebox requests.</param>
/// <param name="ApiVersion">The metadata advertised by the same DNS-SD instance.</param>
public sealed record FreeboxDiscoveryResult(Uri Origin, ApiVersionResponse ApiVersion);
