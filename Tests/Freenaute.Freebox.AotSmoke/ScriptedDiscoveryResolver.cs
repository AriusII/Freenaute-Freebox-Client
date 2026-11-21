namespace Freenaute.Freebox.AotSmoke;

internal sealed class ScriptedDiscoveryResolver(FreeboxDiscoveryResult? result) : IFreeboxDiscoveryResolver
{
	public int Calls { get; private set; }

	public ValueTask<FreeboxDiscoveryResult?> DiscoverAsync(CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		Calls++;
		return ValueTask.FromResult(result);
	}
}
