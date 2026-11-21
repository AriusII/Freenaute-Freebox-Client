namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxPlayer
{
	private readonly string _path;
	private readonly IFreeboxTransport _transport;

	internal FreeboxPlayer(IFreeboxTransport transport, string id)
	{
		_transport = transport;
		_path = $"player/{id}/api/v6/";
	}

	public Task<PlayerStatus> GetStatusAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, _path + "status/",
			ServicesJsonSerializerContext.Default.PlayerStatus, cancellationToken);
	}

	public Task<PlayerVolume> GetVolumeAsync(CancellationToken cancellationToken = default)
	{
		return _transport.SendAsync(HttpMethod.Get, _path + "control/volume/",
			ServicesJsonSerializerContext.Default.PlayerVolume, cancellationToken);
	}

	public Task<PlayerVolume> UpdateVolumeAsync(PlayerVolumePatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return _transport.SendAsync(HttpMethod.Put, _path + "control/volume/", request,
			ServicesJsonSerializerContext.Default.PlayerVolumePatch, ServicesJsonSerializerContext.Default.PlayerVolume,
			cancellationToken);
	}

	public FreeboxPlayerVolumeCommand Volume() => new(new PlayerVolumePatch(), UpdateVolumeAsync);

	public Task SendMediaCommandAsync(PlayerMediaCommandName command, CancellationToken cancellationToken = default)
	{
		if (command.Value is not ("play_pause" or "stop" or "prev" or "next" or "select_stream" or "select_audio_track"
		    or "select_srt_track"))
		{
			throw new ArgumentException("Only documented player media commands are supported.", nameof(command));
		}

		return _transport.SendAsync(HttpMethod.Post, _path + "control/mediactrl/",
			new PlayerMediaCommand { Cmd = command },
			ServicesJsonSerializerContext.Default.PlayerMediaCommand, cancellationToken);
	}

	public Task OpenAsync(string url, string? type = null, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(url);
		return _transport.SendAsync(HttpMethod.Post, _path + "control/open",
			new PlayerOpenRequest { Url = url, Type = type },
			ServicesJsonSerializerContext.Default.PlayerOpenRequest, cancellationToken);
	}
}
