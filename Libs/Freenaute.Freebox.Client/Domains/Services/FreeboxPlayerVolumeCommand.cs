namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxPlayerVolumeCommand
{
	private readonly PlayerVolumePatch _request;
	private readonly Func<PlayerVolumePatch, CancellationToken, Task<PlayerVolume>> _send;

	internal FreeboxPlayerVolumeCommand(PlayerVolumePatch request,
		Func<PlayerVolumePatch, CancellationToken, Task<PlayerVolume>> send)
	{
		_request = request;
		_send = send;
	}

	public FreeboxPlayerVolumeCommand Level(int volume) => new(_request with { Volume = volume }, _send);

	public FreeboxPlayerVolumeCommand Muted(bool mute = true) => new(_request with { Mute = mute }, _send);

	public Task<PlayerVolume> SendAsync(CancellationToken cancellationToken = default) =>
		_send(_request, cancellationToken);
}
