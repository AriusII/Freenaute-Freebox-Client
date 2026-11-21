namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxFreeplugApi
{
    private readonly IFreeboxTransport _transport;

    public FreeboxFreeplugApi(IFreeboxTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;

    public FreeboxFreeplug Plug(string id)
    {
        return new FreeboxFreeplug(_transport, id);
    }

    /// <summary>Get the current Freeplugs networks.</summary>
    /// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-freeplug-</remarks>
    public Task<FreeplugNetwork[]> GetNetworksAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Get, "freeplug/", Json.FreeplugNetworkArray, cancellationToken);
    }
}
