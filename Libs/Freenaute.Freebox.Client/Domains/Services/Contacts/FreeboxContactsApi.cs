namespace Freenaute.Freebox.Client.Domains.Services.Contacts;

public sealed class FreeboxContactsApi(IFreeboxTransport transport)
{
	public Task<ContactEntry[]> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "contact/", ServicesJsonSerializerContext.Default.ContactEntryArray,
			cancellationToken);
	}

	public FreeboxContact Contact(long id) => new(transport, id);

	public Task<ContactEntry> CreateAsync(ContactWriteRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Post, "contact/", request,
			ServicesJsonSerializerContext.Default.ContactWriteRequest,
			ServicesJsonSerializerContext.Default.ContactEntry, cancellationToken);
	}

	public FreeboxServiceCommand<ContactWriteRequest, ContactEntry> Create() =>
		new(new ContactWriteRequest(), CreateAsync);
}
