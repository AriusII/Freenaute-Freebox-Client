namespace Freenaute.Freebox.Client.Domains.SystemHome;

public interface IFreeboxSystemHomeApi
{
    FreeboxSystemApi System { get; }
    FreeboxLanguageApi Language { get; }
    FreeboxLcdApi Lcd { get; }
    FreeboxLedstripApi Ledstrip { get; }
    FreeboxStandbyApi Standby { get; }
    FreeboxUpdatesApi Updates { get; }
    FreeboxHomeApi Home { get; }
    FreeboxProfilesApi Profiles { get; }
}
