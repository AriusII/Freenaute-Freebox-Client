namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxLcdApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxLcdApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<LcdConfig> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "lcd/config/", SystemHomeWire.Json.LcdConfig, cancellationToken);
    }

    public FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig> Configure()
    {
        return new FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig>(transport, HttpMethod.Put, "lcd/config/",
            new LcdConfigPatch(), SystemHomeWire.Json.LcdConfigPatch,
            SystemHomeWire.Json.LcdConfig);
    }

    public FreeboxSystemHomeCommand<LcdConfigPatch, LcdConfig> WithBrightness(int brightness)
    {
        return Configure().UseFields(new LcdConfigPatch().WithBrightness(brightness));
    }
}
