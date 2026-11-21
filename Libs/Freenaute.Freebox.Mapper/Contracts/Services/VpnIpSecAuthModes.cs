namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Preserves the named authentication-mode map or the declared array without discarding map keys.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNIPSecConfig.auth_modes" />
[JsonConverter(typeof(VpnIpSecAuthModesJsonConverter))]
public readonly struct VpnIpSecAuthModes
{
    private readonly Dictionary<string, VpnIpSecAuthMode>? _map;
    private readonly VpnIpSecAuthMode[]? _items;

    private VpnIpSecAuthModes(Dictionary<string, VpnIpSecAuthMode> map)
    {
        Kind = VpnIpSecAuthModesKind.Map;
        _map = map;
        _items = null;
    }

    private VpnIpSecAuthModes(VpnIpSecAuthMode[] items)
    {
        Kind = VpnIpSecAuthModesKind.Array;
        _items = items;
        _map = null;
    }

    public VpnIpSecAuthModesKind Kind { get; }

    public IReadOnlyDictionary<string, VpnIpSecAuthMode> Map => Kind == VpnIpSecAuthModesKind.Map
        ? new ReadOnlyDictionary<string, VpnIpSecAuthMode>(_map!)
        : throw new InvalidOperationException("The authentication modes are not a map.");

    public IReadOnlyList<VpnIpSecAuthMode> Items => Kind == VpnIpSecAuthModesKind.Array
        ? Array.AsReadOnly(_items!)
        : throw new InvalidOperationException("The authentication modes are not an array.");

    public static VpnIpSecAuthModes FromMap(Dictionary<string, VpnIpSecAuthMode> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new VpnIpSecAuthModes(new Dictionary<string, VpnIpSecAuthMode>(value, StringComparer.Ordinal));
    }

    public static VpnIpSecAuthModes FromArray(VpnIpSecAuthMode[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new VpnIpSecAuthModes((VpnIpSecAuthMode[])value.Clone());
    }

    internal Dictionary<string, VpnIpSecAuthMode> WireMap => _map!;
    internal VpnIpSecAuthMode[] WireItems => _items!;

    public override string ToString()
    {
        return nameof(VpnIpSecAuthModes);
    }
}
