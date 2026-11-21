namespace Freenaute.Freebox.Client.Tests;

internal sealed class NonSeekableReadStream(byte[] data) : Stream
{
    private readonly MemoryStream _source = new(data);
    public bool WasDisposed { get; private set; }
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException("Length must not be inferred.");

    public override long Position
    {
        get => throw new NotSupportedException("Position must not be inspected.");
        set => throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _source.Read(buffer, offset, count);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        return _source.ReadAsync(buffer, cancellationToken);
    }

    public override void Flush()
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException("Resume must not seek.");
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
        WasDisposed = true;
        if (disposing)
        {
            _source.Dispose();
        }

        base.Dispose(disposing);
    }
}
