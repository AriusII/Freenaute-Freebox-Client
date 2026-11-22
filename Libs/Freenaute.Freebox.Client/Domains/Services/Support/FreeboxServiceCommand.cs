namespace Freenaute.Freebox.Client.Domains.Services.Support;

/// <summary>An immutable, deferred operation over a typed request. Each call to With creates an independent command.</summary>
public sealed class FreeboxServiceCommand<TRequest, TResponse>
{
	private readonly Func<TRequest, CancellationToken, Task<TResponse>> _send;

	internal FreeboxServiceCommand(TRequest request, Func<TRequest, CancellationToken, Task<TResponse>> send)
	{
		Request = request;
		_send = send;
	}

	public TRequest Request { get; }

	public FreeboxServiceCommand<TRequest, TResponse> With(Func<TRequest, TRequest> configure)
	{
		ArgumentNullException.ThrowIfNull(configure);
		var request = configure(Request);
		ArgumentNullException.ThrowIfNull(request);
		return new FreeboxServiceCommand<TRequest, TResponse>(request, _send);
	}

	public async Task<TResponse> SendAsync(CancellationToken cancellationToken = default) =>
		await _send(Request, cancellationToken).ConfigureAwait(false);
}

public sealed class FreeboxServiceCommand<TRequest>
{
	private readonly Func<TRequest, CancellationToken, Task> _send;

	internal FreeboxServiceCommand(TRequest request, Func<TRequest, CancellationToken, Task> send)
	{
		Request = request;
		_send = send;
	}

	public TRequest Request { get; }

	public FreeboxServiceCommand<TRequest> With(Func<TRequest, TRequest> configure)
	{
		ArgumentNullException.ThrowIfNull(configure);
		var request = configure(Request);
		ArgumentNullException.ThrowIfNull(request);
		return new FreeboxServiceCommand<TRequest>(request, _send);
	}

	public async Task SendAsync(CancellationToken cancellationToken = default) =>
		await _send(Request, cancellationToken).ConfigureAwait(false);
}
