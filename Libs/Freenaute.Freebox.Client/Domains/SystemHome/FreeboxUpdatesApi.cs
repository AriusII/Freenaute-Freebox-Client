namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxUpdatesApi
{
	private readonly IFreeboxTransport transport;

	internal FreeboxUpdatesApi(IFreeboxTransport transport)
	{
		this.transport = transport;
	}

	public Task<UpdateStatus> GetAsync(CancellationToken cancellationToken = default) =>
		transport.SendAsync(HttpMethod.Get, "update/", SystemHomeWire.Json.UpdateStatus, cancellationToken);
}
