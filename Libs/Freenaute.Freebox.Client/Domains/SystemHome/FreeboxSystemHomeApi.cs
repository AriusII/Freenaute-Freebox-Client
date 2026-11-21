namespace Freenaute.Freebox.Client.Domains.SystemHome;

/// <summary>System and Home facades sharing one injected transport and its session coordinator.</summary>
public sealed class FreeboxSystemHomeApi : IFreeboxSystemHomeApi
{
    public FreeboxSystemHomeApi(IFreeboxTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        System = new FreeboxSystemApi(transport);
        Language = new FreeboxLanguageApi(transport);
        Lcd = new FreeboxLcdApi(transport);
        Ledstrip = new FreeboxLedstripApi(transport);
        Standby = new FreeboxStandbyApi(transport);
        Updates = new FreeboxUpdatesApi(transport);
        Home = new FreeboxHomeApi(transport);
        Profiles = new FreeboxProfilesApi(transport);
    }

    public FreeboxSystemApi System { get; }
    public FreeboxLanguageApi Language { get; }
    public FreeboxLcdApi Lcd { get; }
    public FreeboxLedstripApi Ledstrip { get; }
    public FreeboxStandbyApi Standby { get; }
    public FreeboxUpdatesApi Updates { get; }
    public FreeboxHomeApi Home { get; }
    public FreeboxProfilesApi Profiles { get; }
}
