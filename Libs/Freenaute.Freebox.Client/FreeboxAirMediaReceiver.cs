namespace Freenaute.Freebox.Client;

/// <summary>An immutable fluent resource selector for one AirMedia receiver.</summary>
public sealed class FreeboxAirMediaReceiver
{
    private readonly IFreeboxAirMediaApi _api;
    private readonly string _receiverName;

    internal FreeboxAirMediaReceiver(IFreeboxAirMediaApi api, string receiverName)
    {
        FreeboxApiPath.EncodeSegment(receiverName);
        _api = api;
        _receiverName = receiverName;
    }

    public FreeboxAirMediaCommand PlayVideo(string media)
    {
        return CreateCommand(AirMediaMediaType.Video, media);
    }

    public FreeboxAirMediaCommand ShowPhoto(string media)
    {
        return CreateCommand(AirMediaMediaType.Photo, media);
    }

    public FreeboxAirMediaCommand StopVideo()
    {
        return CreateStopCommand(AirMediaMediaType.Video);
    }

    public FreeboxAirMediaCommand StopPhoto()
    {
        return CreateStopCommand(AirMediaMediaType.Photo);
    }

    private FreeboxAirMediaCommand CreateStopCommand(AirMediaMediaType mediaType)
    {
        return new FreeboxAirMediaCommand(_api, _receiverName,
            new AirMediaReceiverRequest(AirMediaAction.Stop, mediaType));
    }

    private FreeboxAirMediaCommand CreateCommand(AirMediaMediaType mediaType, string media)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(media);
        return new FreeboxAirMediaCommand(_api, _receiverName,
            new AirMediaReceiverRequest(AirMediaAction.Start, mediaType, media));
    }
}
