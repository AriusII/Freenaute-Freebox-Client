namespace Freenaute.Freebox.Client.Domains.SystemHome;

public sealed class FreeboxLanguageApi
{
    private readonly IFreeboxTransport transport;

    internal FreeboxLanguageApi(IFreeboxTransport transport)
    {
        this.transport = transport;
    }

    public Task<LanguageSupport> GetAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "lang/", SystemHomeWire.Json.LanguageSupport, cancellationToken);
    }

    public FreeboxSystemHomeCommand<SetLanguageRequest> Set(string language)
    {
        return new FreeboxSystemHomeCommand<SetLanguageRequest>(transport, HttpMethod.Post, "lang/",
            new SetLanguageRequest(language), SystemHomeWire.Json.SetLanguageRequest);
    }
}
