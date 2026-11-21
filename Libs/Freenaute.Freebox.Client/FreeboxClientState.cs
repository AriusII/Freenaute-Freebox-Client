namespace Freenaute.Freebox.Client;

internal sealed class FreeboxClientState : IDisposable
{
	internal Uri? ApiAddress;
	internal Uri? ApiOrigin;
	internal ApiVersionResponse? ApiVersion;
	internal SessionResponse? Session;

	internal FreeboxClientState(FreeboxClientOptions options, IFreeboxCredentialStore credentialStore,
		IFreeboxDiscoveryResolver discoveryResolver, FreeboxAppCredentials? standaloneCredentials = null)
	{
		Options = options;
		CredentialStore = credentialStore;
		DiscoveryResolver = discoveryResolver;
		StandaloneCredentials = standaloneCredentials;
	}

	internal FreeboxClientOptions Options { get; }
	internal IFreeboxCredentialStore CredentialStore { get; }
	internal IFreeboxDiscoveryResolver DiscoveryResolver { get; }
	internal FreeboxAppCredentials? StandaloneCredentials { get; }
	internal SemaphoreSlim DiscoveryGate { get; } = new(1, 1);
	internal SemaphoreSlim SessionGate { get; } = new(1, 1);

	public void Dispose()
	{
		DiscoveryGate.Dispose();
		SessionGate.Dispose();
	}

	internal FreeboxCredentialId GetCredentialId()
	{
		if (string.IsNullOrWhiteSpace(Options.ApplicationId))
		{
			throw new FreeboxAuthenticationException("ApplicationId must be configured for Freebox authorization.");
		}

		if (StandaloneCredentials is null && string.IsNullOrWhiteSpace(ApiVersion?.Uid))
		{
			throw new FreeboxAuthenticationException(
				"Discovery must provide a stable Freebox UID before loading persistent credentials.");
		}

		return new FreeboxCredentialId(Options.CredentialProfile, Options.ApplicationId, ApiVersion?.Uid);
	}
}
