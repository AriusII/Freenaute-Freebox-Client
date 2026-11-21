namespace Freenaute.Freebox.Mapper.Contracts.Services;

/// <summary>Exact wire values documented for VPNOpenVpnConfig.cipher; unknown incoming strings are preserved.</summary>
/// <seealso href="http://mafreebox.freebox.fr/doc/index.html#VPNOpenVpnConfig.cipher" />
[JsonConverter(typeof(VpnCipherJsonConverter))]
public readonly record struct VpnCipher(string Value)
{
    public static VpnCipher Blowfish => new("blowfish");
    public static VpnCipher Aes128 => new("aes128");
    public static VpnCipher Aes256 => new("aes256");
    public static VpnCipher Chacha20poly1305 => new("chacha20poly1305");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}
