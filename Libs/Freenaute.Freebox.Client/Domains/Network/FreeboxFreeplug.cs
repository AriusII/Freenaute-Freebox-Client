namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxFreeplug
{
    private readonly string _id;
    private readonly IFreeboxTransport _transport;

    public FreeboxFreeplug(IFreeboxTransport transport, string id)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
        _id = FreeboxApiPath.EncodeSegment(id);
    }

    private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


    /// <summary>Get a particular Freeplug information.</summary>
    /// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-freeplug-id-</remarks>
    public Task<Freeplug> GetAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Get, $"freeplug/{_id}/", Json.Freeplug, cancellationToken);
    }

    /// <summary>Reset a Freeplug.</summary>
    /// <remarks>http://mafreebox.freebox.fr/doc/index.html#post--api-v8-freeplug-id-reset-</remarks>
    public Task ResetAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Post, $"freeplug/{_id}/reset/", cancellationToken);
    }
}
