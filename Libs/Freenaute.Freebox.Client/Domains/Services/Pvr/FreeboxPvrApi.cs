using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Pvr;

/// <summary>PVR API (internal Freebox API routes).</summary>
public sealed class FreeboxPvrApi(IFreeboxTransport transport)
{
	/// <summary>Get PVR configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     INTERNAL USE ONLY according to Freebox documentation.
	/// </remarks>
	public async Task<PvrConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.Configuration, ServicesJsonSerializerContext.Default.PvrConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update PVR configuration.</summary>
	/// <remarks>
	///     <b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.
	///     INTERNAL USE ONLY according to Freebox documentation.
	/// </remarks>
	public async Task UpdateConfigurationAsync(PvrConfig request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		await transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Pvr.Configuration, request,
			ServicesJsonSerializerContext.Default.PvrConfig,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List finished records.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Frecord[]> GetFinishedAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.Finished,
			ServicesJsonSerializerContext.Default.FrecordArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one finished record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Frecord> GetFinishedAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.FinishedItem(ServicesPath.Id(id)),
			ServicesJsonSerializerContext.Default.Frecord,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one finished record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Frecord> UpdateFinishedAsync(long id, Frecord request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Pvr.FinishedItem(ServicesPath.Id(id)), request,
			ServicesJsonSerializerContext.Default.Frecord,
			ServicesJsonSerializerContext.Default.Frecord, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete one finished record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task DeleteFinishedAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, ServicesConstants.Routes.Pvr.FinishedItem(ServicesPath.Id(id)), cancellationToken)
			.ConfigureAwait(false);

	/// <summary>List PVR media.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<PvrMedia[]> GetMediaAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.Media,
			ServicesJsonSerializerContext.Default.PvrMediaArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>List programmed records.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Precord[]> GetProgrammedAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.Programmed,
			ServicesJsonSerializerContext.Default.PrecordArray,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Create a programmed record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Precord> CreateProgrammedAsync(Precord request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Pvr.Programmed, request,
			ServicesJsonSerializerContext.Default.Precord,
			ServicesJsonSerializerContext.Default.Precord, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Get one programmed record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Precord> GetProgrammedAsync(long id, CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.ProgrammedItem(ServicesPath.Id(id)),
			ServicesJsonSerializerContext.Default.Precord,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Update one programmed record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<Precord> UpdateProgrammedAsync(long id, Precord request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return await transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Pvr.ProgrammedItem(ServicesPath.Id(id)), request,
			ServicesJsonSerializerContext.Default.Precord,
			ServicesJsonSerializerContext.Default.Precord, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Delete one programmed record.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task DeleteProgrammedAsync(long id, CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Delete, ServicesConstants.Routes.Pvr.ProgrammedItem(ServicesPath.Id(id)), cancellationToken)
			.ConfigureAwait(false);

	/// <summary>Get PVR quota.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<PvrQuota> GetQuotaAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Pvr.Quota, ServicesJsonSerializerContext.Default.PvrQuota,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Request the next PVR quota threshold.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<PvrQuota> UpdateQuotaAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Pvr.Quota, new PvrQuotaRequest(),
			ServicesJsonSerializerContext.Default.PvrQuotaRequest, ServicesJsonSerializerContext.Default.PvrQuota,
			cancellationToken).ConfigureAwait(false);
	}
}
