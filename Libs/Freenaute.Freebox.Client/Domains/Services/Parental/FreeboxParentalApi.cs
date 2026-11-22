using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Parental;

/// <summary>Parental control API backed by the documented <c>parental/</c> routes.</summary>
public sealed class FreeboxParentalApi(IFreeboxTransport transport) : IFreeboxParentalApi
{
	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#get--api-v4-parental-config-" />
	public async Task<ParentalFilterConfig> GetConfigAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Parental.Configuration,
			ServicesJsonSerializerContext.Default.ParentalFilterConfig, cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#put--api-v4-parental-config-" />
	public Task<ParentalFilterConfig> UpdateConfigAsync(ParentalFilterConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, ServicesConstants.Routes.Parental.Configuration, request,
			ServicesJsonSerializerContext.Default.ParentalFilterConfigPatch,
			ServicesJsonSerializerContext.Default.ParentalFilterConfig, cancellationToken);
	}

	public FreeboxServiceCommand<ParentalFilterConfigPatch, ParentalFilterConfig> Configure() =>
		new(new ParentalFilterConfigPatch(), UpdateConfigAsync);

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#get--api-v4-parental-filter-" />
	public async Task<ParentalFilter[]> GetFiltersAsync(CancellationToken cancellationToken = default)
	{
		return await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Parental.Filters,
			ServicesJsonSerializerContext.Default.ParentalFilterArray, cancellationToken).ConfigureAwait(false);
	}

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#post--api-v4-parental-filter-" />
	public Task<ParentalFilter> CreateFilterAsync(ParentalFilterWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Parental.Filters, request,
			ServicesJsonSerializerContext.Default.ParentalFilterWriteRequest,
			ServicesJsonSerializerContext.Default.ParentalFilter, cancellationToken);
	}

	public FreeboxServiceCommand<ParentalFilterWriteRequest, ParentalFilter> CreateFilter() =>
		new(new ParentalFilterWriteRequest(), CreateFilterAsync);

	public FreeboxParentalFilter Filter(long id) => new(transport, id);
}
