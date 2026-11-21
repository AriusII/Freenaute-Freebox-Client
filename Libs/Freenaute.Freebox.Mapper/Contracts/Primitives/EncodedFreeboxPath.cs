namespace Freenaute.Freebox.Mapper.Contracts.Primitives;

/// <summary>
///     Preserves the exact Base64 path returned by Freebox, without Unicode or Base64 normalization.
///     Clear filenames and plaintext task descriptions are separate contracts.
/// </summary>
[JsonConverter(typeof(EncodedFreeboxPathJsonConverter))]
public readonly struct EncodedFreeboxPath : IEquatable<EncodedFreeboxPath>
{
    private static readonly UTF8Encoding StrictUtf8 = new(false, true);
    private readonly string? _encoded;

    private EncodedFreeboxPath(string encoded)
    {
        _encoded = encoded;
    }

    public bool IsInitialized => _encoded is not null;
    public string Value => _encoded ?? throw new InvalidOperationException("This Freebox path is not initialized.");

    /// <summary>Validates Base64 syntax and retains the original wire string exactly.</summary>
    public static EncodedFreeboxPath FromEncoded(string encoded)
    {
        ArgumentNullException.ThrowIfNull(encoded);
        _ = Convert.FromBase64String(encoded);
        return new EncodedFreeboxPath(encoded);
    }

    /// <summary>Encodes the supplied UTF8 path without NFC/NFD normalization or replacement of invalid characters.</summary>
    public static EncodedFreeboxPath FromUtf8Path(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return new EncodedFreeboxPath(Convert.ToBase64String(StrictUtf8.GetBytes(path)));
    }

    /// <summary>Decodes UTF8 strictly; never rewrites the stored wire value.</summary>
    public string DecodeUtf8Path()
    {
        return StrictUtf8.GetString(Convert.FromBase64String(Value));
    }

    public byte[] DecodeBytes()
    {
        return Convert.FromBase64String(Value);
    }

    public string ToEscapedSegment()
    {
        return Uri.EscapeDataString(Value);
    }

    public bool Equals(EncodedFreeboxPath other)
    {
        return string.Equals(_encoded, other._encoded, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is EncodedFreeboxPath other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _encoded is null ? 0 : StringComparer.Ordinal.GetHashCode(_encoded);
    }

    public static bool operator ==(EncodedFreeboxPath left, EncodedFreeboxPath right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EncodedFreeboxPath left, EncodedFreeboxPath right)
    {
        return !left.Equals(right);
    }
}
