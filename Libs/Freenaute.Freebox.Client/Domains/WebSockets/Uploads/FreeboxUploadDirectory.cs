namespace Freenaute.Freebox.Client.Domains.WebSockets.Uploads;

public sealed class FreeboxUploadDirectory
{
	private readonly EncodedFreeboxPath _directory;
	private readonly IFreeboxWebSocketTransport _transport;

	public FreeboxUploadDirectory(IFreeboxWebSocketTransport transport, EncodedFreeboxPath directory)
	{
		ArgumentNullException.ThrowIfNull(transport);
		if (!directory.IsInitialized)
		{
			throw new ArgumentException("A destination requires an encoded Freebox path.", nameof(directory));
		}

		_transport = transport;
		_directory = directory;
	}

	public FreeboxUploadCommand File(string filename) => new(_transport, _directory, filename);
}
