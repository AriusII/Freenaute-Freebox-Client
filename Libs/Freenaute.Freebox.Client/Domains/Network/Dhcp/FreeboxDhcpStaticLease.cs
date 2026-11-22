using Freenaute.Freebox.Client.Domains.Network.Support;

namespace Freenaute.Freebox.Client.Domains.Network.Dhcp;

/// <summary>Typed network operations using the currently discovered outer API major.</summary>
public sealed class FreeboxDhcpStaticLease
{
	private readonly string _id;
	private readonly IFreeboxTransport _transport;

	public FreeboxDhcpStaticLease(IFreeboxTransport transport, string id)
	{
		ArgumentNullException.ThrowIfNull(transport);
		ArgumentException.ThrowIfNullOrWhiteSpace(id);
		_transport = transport;
		_id = FreeboxApiPath.EncodeSegment(id);
	}

	private static NetworkJsonSerializerContext Json => NetworkJsonSerializerContext.Default;


	/// <summary>Get a given DHCP static lease.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#get--api-v16-dhcp-static_lease-id</remarks>
	public async Task<DhcpStaticLease> GetAsync(CancellationToken cancellationToken = default)
	{
		return await _transport.SendAsync(HttpMethod.Get, NetworkConstants.Routes.Dhcp.StaticLease(_id), Json.DhcpStaticLease,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update DHCP static lease.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#put--api-v16-dhcp-static_lease-id</remarks>
	public Task<DhcpStaticLease> UpdateAsync(DhcpStaticLeasePatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		NetworkWriteValidation.Validate(request);
		return _transport.SendAsync(HttpMethod.Put, NetworkConstants.Routes.Dhcp.StaticLease(_id), request, Json.DhcpStaticLeasePatch,
			Json.DhcpStaticLease, cancellationToken);
	}

	/// <summary>Delete a DHCP static lease.</summary>
	/// <remarks>http://mafreebox.freebox.fr/doc/index.html#delete--api-v8-dhcp-static_lease-id</remarks>
	public async Task DeleteAsync(CancellationToken cancellationToken = default) =>
		await _transport.SendAsync(HttpMethod.Delete, NetworkConstants.Routes.Dhcp.StaticLease(_id), cancellationToken)
			.ConfigureAwait(false);
}
