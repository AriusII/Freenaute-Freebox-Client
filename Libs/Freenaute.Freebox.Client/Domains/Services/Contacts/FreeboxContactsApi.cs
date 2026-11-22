using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Contacts;

public sealed class FreeboxContactsApi(IFreeboxTransport transport)
{
	public async Task<ContactEntry[]> GetAllAsync(CancellationToken cancellationToken = default) =>
		await GetAllAsync(options: null, cancellationToken).ConfigureAwait(false);

	public async Task<ContactEntry[]> GetAllAsync(ContactListOptions? options,
		CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, BuildContactListPath(options),
			ServicesJsonSerializerContext.Default.ContactEntryArray, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxContact Contact(long id) => new(transport, id);

	/// <summary>Create a number using the documented bracket-style route /[number,address,url,email]/.</summary>
	public async Task<ContactNumber> CreateNumberAsync(ContactNumberWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Contacts.Numbers, request,
			ServicesJsonSerializerContext.Default.ContactNumberWriteRequest,
			ServicesJsonSerializerContext.Default.ContactNumber, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxServiceCommand<ContactNumberWriteRequest, ContactNumber> CreateNumber() =>
		new(new ContactNumberWriteRequest(), CreateNumberAsync);

	/// <summary>Access one number by id using the documented bracket-style route /[...]/{id}.</summary>
	public FreeboxServiceResource<ContactNumber, ContactNumberWriteRequest> Number(long id) =>
		new(transport, $"number/{ServicesPath.Id(id)}", ServicesJsonSerializerContext.Default.ContactNumber,
			ServicesJsonSerializerContext.Default.ContactNumberWriteRequest, request => request.Validate());

	/// <summary>Create an address using the documented bracket-style route /[number,address,url,email]/.</summary>
	public async Task<ContactAddress> CreateAddressAsync(ContactAddressWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Contacts.Addresses, request,
			ServicesJsonSerializerContext.Default.ContactAddressWriteRequest,
			ServicesJsonSerializerContext.Default.ContactAddress, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxServiceCommand<ContactAddressWriteRequest, ContactAddress> CreateAddress() =>
		new(new ContactAddressWriteRequest(), CreateAddressAsync);

	/// <summary>Access one address by id using the documented bracket-style route /[...]/{id}.</summary>
	public FreeboxServiceResource<ContactAddress, ContactAddressWriteRequest> Address(long id) =>
		new(transport, $"address/{ServicesPath.Id(id)}", ServicesJsonSerializerContext.Default.ContactAddress,
			ServicesJsonSerializerContext.Default.ContactAddressWriteRequest, request => request.Validate());

	/// <summary>Create a URL using the documented bracket-style route /[number,address,url,email]/.</summary>
	public async Task<ContactUrl> CreateUrlAsync(ContactUrlWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Contacts.Urls, request,
			ServicesJsonSerializerContext.Default.ContactUrlWriteRequest,
			ServicesJsonSerializerContext.Default.ContactUrl, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxServiceCommand<ContactUrlWriteRequest, ContactUrl> CreateUrl() =>
		new(new ContactUrlWriteRequest(), CreateUrlAsync);

	/// <summary>Access one URL by id using the documented bracket-style route /[...]/{id}.</summary>
	public FreeboxServiceResource<ContactUrl, ContactUrlWriteRequest> Url(long id) =>
		new(transport, $"url/{ServicesPath.Id(id)}", ServicesJsonSerializerContext.Default.ContactUrl,
			ServicesJsonSerializerContext.Default.ContactUrlWriteRequest, request => request.Validate());

	/// <summary>Create an email using the documented bracket-style route /[number,address,url,email]/.</summary>
	public async Task<ContactEmail> CreateEmailAsync(ContactEmailWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Contacts.Emails, request,
			ServicesJsonSerializerContext.Default.ContactEmailWriteRequest,
			ServicesJsonSerializerContext.Default.ContactEmail, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxServiceCommand<ContactEmailWriteRequest, ContactEmail> CreateEmail() =>
		new(new ContactEmailWriteRequest(), CreateEmailAsync);

	/// <summary>Access one email by id using the documented bracket-style route /[...]/{id}.</summary>
	public FreeboxServiceResource<ContactEmail, ContactEmailWriteRequest> Email(long id) =>
		new(transport, $"email/{ServicesPath.Id(id)}", ServicesJsonSerializerContext.Default.ContactEmail,
			ServicesJsonSerializerContext.Default.ContactEmailWriteRequest, request => request.Validate());

	public async Task<ContactEntry> CreateAsync(ContactWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Contacts.Collection, request,
			ServicesJsonSerializerContext.Default.ContactWriteRequest,
			ServicesJsonSerializerContext.Default.ContactEntry, cancellationToken).ConfigureAwait(false);
	}

	public FreeboxServiceCommand<ContactWriteRequest, ContactEntry> Create() =>
		new(new ContactWriteRequest(), CreateAsync);

	/// <summary>Get numbers for a contact (documented alias: /contact/{contact_id}/[numbers|...]/).</summary>
	public async Task<ContactNumber[]> GetNumbersAsync(long contactId, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Contacts.ContactNumbers(ServicesPath.Id(contactId)),
			ServicesJsonSerializerContext.Default.ContactNumberArray, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get addresses for a contact (documented alias: /contact/{contact_id}/[...|addresses|...]/).</summary>
	public Task<ContactAddress[]> GetAddressesAsync(long contactId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Contacts.ContactAddresses(ServicesPath.Id(contactId)),
			ServicesJsonSerializerContext.Default.ContactAddressArray, cancellationToken);
	}

	/// <summary>Get urls for a contact (documented alias: /contact/{contact_id}/[...|urls|...]/).</summary>
	public async Task<ContactUrl[]> GetUrlsAsync(long contactId, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Contacts.ContactUrls(ServicesPath.Id(contactId)),
			ServicesJsonSerializerContext.Default.ContactUrlArray, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get emails for a contact (documented alias: /contact/{contact_id}/[...|emails]/).</summary>
	public Task<ContactEmail[]> GetEmailsAsync(long contactId, CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Contacts.ContactEmails(ServicesPath.Id(contactId)),
			ServicesJsonSerializerContext.Default.ContactEmailArray, cancellationToken);
	}

	/// <summary>Get one contact item collection by kind: numbers, addresses, urls or emails.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v8-contact-contact_id-numbers-addresses-urls-emails-</remarks>
	public async Task<JsonElement> GetItemsAsync(long contactId, string kind,
		CancellationToken cancellationToken = default)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(contactId);
		if (kind is not "numbers" and not "addresses" and not "urls" and not "emails")
		{
			throw new ArgumentException("Kind must be one of: numbers, addresses, urls, emails.", nameof(kind));
		}

		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Contacts.ContactResource(ServicesPath.Id(contactId), kind),
			ServicesJsonSerializerContext.Default.JsonElement, cancellationToken).ConfigureAwait(false);
	}

	private static string BuildContactListPath(ContactListOptions? options)
	{
		if (options is null)
		{
			return ServicesConstants.Routes.Contacts.Collection;
		}

		if (options.Start is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(options.Start), "Start cannot be negative.");
		}

		if (options.Limit is < -1)
		{
			throw new ArgumentOutOfRangeException(nameof(options.Limit), "Limit must be -1 or greater.");
		}

		if (options.GroupId is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(options.GroupId), "GroupId cannot be negative.");
		}

		List<string> values = [];
		if (options.Start is not null)
		{
			values.Add("start=" + Uri.EscapeDataString(options.Start.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (options.Limit is not null)
		{
			values.Add("limit=" + Uri.EscapeDataString(options.Limit.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (options.GroupId is not null)
		{
			values.Add("group_id=" +
			           Uri.EscapeDataString(options.GroupId.Value.ToString(CultureInfo.InvariantCulture)));
		}

		return values.Count is 0
			? ServicesConstants.Routes.Contacts.Collection
			: ServicesConstants.Routes.Contacts.Collection + "?" + string.Join('&', values);
	}
}
