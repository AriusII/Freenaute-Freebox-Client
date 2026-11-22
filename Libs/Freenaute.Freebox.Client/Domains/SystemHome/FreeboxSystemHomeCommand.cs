namespace Freenaute.Freebox.Client.Domains.SystemHome;

/// <summary>An immutable success-only command; construction performs no network I/O.</summary>
public sealed class FreeboxSystemHomeCommand<TRequest> where TRequest : ISystemHomeRequest
{
	private readonly HttpMethod _method;
	private readonly string _path;
	private readonly JsonTypeInfo<TRequest> _requestType;
	private readonly IFreeboxTransport _transport;

	internal FreeboxSystemHomeCommand(IFreeboxTransport transport, HttpMethod method, string path,
		TRequest fields, JsonTypeInfo<TRequest> requestType)
	{
		_transport = transport;
		_method = method;
		_path = path;
		Fields = fields;
		_requestType = requestType;
	}

	public TRequest Fields { get; }

	public FreeboxSystemHomeCommand<TRequest> UseFields(TRequest fields)
	{
		ArgumentNullException.ThrowIfNull(fields);
		return new FreeboxSystemHomeCommand<TRequest>(_transport, _method, _path, fields, _requestType);
	}

	public Task SendAsync(CancellationToken cancellationToken = default)
	{
		Fields.Validate();
		return _transport.SendAsync(_method, _path, Fields, _requestType, cancellationToken);
	}
}

/// <summary>An immutable command with a source-generated result contract.</summary>
public sealed class FreeboxSystemHomeCommand<TRequest, TResponse> where TRequest : ISystemHomeRequest
{
	private readonly HttpMethod _method;
	private readonly string _path;
	private readonly JsonTypeInfo<TRequest> _requestType;
	private readonly JsonTypeInfo<TResponse> _responseType;
	private readonly IFreeboxTransport _transport;

	internal FreeboxSystemHomeCommand(IFreeboxTransport transport, HttpMethod method, string path,
		TRequest fields, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResponse> responseType)
	{
		_transport = transport;
		_method = method;
		_path = path;
		Fields = fields;
		_requestType = requestType;
		_responseType = responseType;
	}

	public TRequest Fields { get; }

	public FreeboxSystemHomeCommand<TRequest, TResponse> UseFields(TRequest fields)
	{
		ArgumentNullException.ThrowIfNull(fields);
		return new FreeboxSystemHomeCommand<TRequest, TResponse>(_transport, _method, _path, fields, _requestType,
			_responseType);
	}

	public Task<TResponse> SendAsync(CancellationToken cancellationToken = default)
	{
		Fields.Validate();
		return _transport.SendAsync(_method, _path, Fields, _requestType, _responseType, cancellationToken);
	}
}
