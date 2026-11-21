namespace Freenaute.Freebox.Client;

/// <summary>Resolves a Freebox DNS-SD service without exposing protocol-specific types.</summary>
public interface IFreeboxDiscoveryResolver
{
	/// <summary>Returns the bounded, trusted discovery candidate, or <see langword="null" /> when none is found.</summary>
	/// <param name="cancellationToken">Cancels the multicast discovery operation.</param>
	ValueTask<FreeboxDiscoveryResult?> DiscoverAsync(CancellationToken cancellationToken = default);
}
