namespace Freenaute.Freebox.Client.Domains.Services;

/// <summary>
///     A contact's item collection. Individual item routes are the documented top-level number/address/url/email
///     routes.
/// </summary>
public sealed class FreeboxContactItems<TRead, TWrite> where TWrite : class, new()
{
	private readonly Func<TWrite, TWrite> _bindContact;
	private readonly string _itemRoot;
	private readonly string _listPath;
	private readonly JsonTypeInfo<TRead[]> _listType;
	private readonly JsonTypeInfo<TRead> _readType;
	private readonly IFreeboxTransport _transport;
	private readonly Action<TWrite> _validate;
	private readonly JsonTypeInfo<TWrite> _writeType;

	internal FreeboxContactItems(IFreeboxTransport transport, string listPath, string itemRoot,
		JsonTypeInfo<TRead[]> listType, JsonTypeInfo<TRead> readType, JsonTypeInfo<TWrite> writeType,
		Action<TWrite> validate, Func<TWrite, TWrite> bindContact)
	{
		_transport = transport;
		_listPath = listPath;
		_itemRoot = itemRoot;
		_listType = listType;
		_readType = readType;
		_writeType = writeType;
		_validate = validate;
		_bindContact = bindContact;
	}

	public Task<TRead[]> GetAllAsync(CancellationToken cancellationToken = default) =>
		_transport.SendAsync(HttpMethod.Get, _listPath, _listType, cancellationToken);

	public FreeboxServiceResource<TRead, TWrite> Item(long id)
	{
		return new FreeboxServiceResource<TRead, TWrite>(_transport, _itemRoot + ServicesPath.Id(id), _readType,
			_writeType, _validate);
	}

	public Task<TRead> CreateAsync(TWrite request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		var bound = _bindContact(request);
		_validate(bound);
		return _transport.SendAsync(HttpMethod.Post, _itemRoot, bound, _writeType, _readType, cancellationToken);
	}

	public FreeboxServiceCommand<TWrite, TRead> Create() => new(new TWrite(), CreateAsync);
}
