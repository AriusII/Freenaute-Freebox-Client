namespace Freenaute.Freebox.Client.Domains.Network;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxWifiGuestKey
{
    private readonly string _key_id;
    private readonly IFreeboxTransport _transport;

    public FreeboxWifiGuestKey(IFreeboxTransport transport, long key_id)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
        _key_id = FreeboxApiPath.EncodeSegment(NetworkPath.Number(key_id));
    }

    private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


    /// <summary>Getting a particular wifi custom key.</summary>
    /// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v9-wifi-custom_key-key_id</remarks>
    public Task<WifiCustomKey> GetAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Get, $"wifi/custom_key/{_key_id}", Json.WifiCustomKey,
            cancellationToken);
    }

    /// <summary>Delete a wifi custom key.</summary>
    /// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v9-wifi-custom_key-key_id</remarks>
    public Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return _transport.SendAsync(HttpMethod.Delete, $"wifi/custom_key/{_key_id}", cancellationToken);
    }
}
