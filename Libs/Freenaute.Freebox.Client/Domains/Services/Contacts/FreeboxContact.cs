using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Contacts;

public sealed class FreeboxContact
{
	private readonly FreeboxServiceResource<ContactEntry, ContactWriteRequest> _resource;

	internal FreeboxContact(IFreeboxTransport transport, long id)
	{
		string segment = ServicesPath.Id(id);
		_resource = new FreeboxServiceResource<ContactEntry, ContactWriteRequest>(transport, $"contact/{segment}",
			ServicesJsonSerializerContext.Default.ContactEntry,
			ServicesJsonSerializerContext.Default.ContactWriteRequest, request => request.Validate());
		Numbers = new FreeboxContactItems<ContactNumber, ContactNumberWriteRequest>(transport,
			$"contact/{segment}/numbers/", ServicesConstants.Routes.Contacts.Numbers, ServicesJsonSerializerContext.Default.ContactNumberArray,
			ServicesJsonSerializerContext.Default.ContactNumber,
			ServicesJsonSerializerContext.Default.ContactNumberWriteRequest,
			request => request.Validate(), request => request with { ContactId = id });
		Addresses = new FreeboxContactItems<ContactAddress, ContactAddressWriteRequest>(transport,
			$"contact/{segment}/addresses/", ServicesConstants.Routes.Contacts.Addresses, ServicesJsonSerializerContext.Default.ContactAddressArray,
			ServicesJsonSerializerContext.Default.ContactAddress,
			ServicesJsonSerializerContext.Default.ContactAddressWriteRequest,
			request => request.Validate(), request => request with { ContactId = id });
		Urls = new FreeboxContactItems<ContactUrl, ContactUrlWriteRequest>(transport, $"contact/{segment}/urls/",
			ServicesConstants.Routes.Contacts.Urls, ServicesJsonSerializerContext.Default.ContactUrlArray,
			ServicesJsonSerializerContext.Default.ContactUrl,
			ServicesJsonSerializerContext.Default.ContactUrlWriteRequest,
			request => request.Validate(), request => request with { ContactId = id });
		Emails = new FreeboxContactItems<ContactEmail, ContactEmailWriteRequest>(transport,
			$"contact/{segment}/emails/", ServicesConstants.Routes.Contacts.Emails, ServicesJsonSerializerContext.Default.ContactEmailArray,
			ServicesJsonSerializerContext.Default.ContactEmail,
			ServicesJsonSerializerContext.Default.ContactEmailWriteRequest,
			request => request.Validate(), request => request with { ContactId = id });
	}

	public FreeboxContactItems<ContactNumber, ContactNumberWriteRequest> Numbers { get; }
	public FreeboxContactItems<ContactAddress, ContactAddressWriteRequest> Addresses { get; }
	public FreeboxContactItems<ContactUrl, ContactUrlWriteRequest> Urls { get; }
	public FreeboxContactItems<ContactEmail, ContactEmailWriteRequest> Emails { get; }

	public async Task<ContactEntry> GetAsync(CancellationToken cancellationToken = default) =>
		await _resource.GetAsync(cancellationToken).ConfigureAwait(false);

	public async Task<ContactEntry> UpdateAsync(ContactWriteRequest request,
		CancellationToken cancellationToken = default) =>
		await _resource.UpdateAsync(request, cancellationToken).ConfigureAwait(false);

	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _resource.DeleteAsync(cancellationToken).ConfigureAwait(false);

	public FreeboxServiceCommand<ContactWriteRequest, ContactEntry> Configure() => _resource.Configure();
}
