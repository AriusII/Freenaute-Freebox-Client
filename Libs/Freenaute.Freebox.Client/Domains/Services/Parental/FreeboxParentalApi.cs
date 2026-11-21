namespace Freenaute.Freebox.Client.Domains.Services.Parental;

/// <summary>Parental control API backed by the documented <c>parental/</c> routes.</summary>
public sealed class FreeboxParentalApi(IFreeboxTransport transport) : IFreeboxParentalApi
{
	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#get--api-v4-parental-config-" />
	public Task<ParentalFilterConfig> GetConfigAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "parental/config/",
			ServicesJsonSerializerContext.Default.ParentalFilterConfig, cancellationToken);
	}

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#put--api-v4-parental-config-" />
	public Task<ParentalFilterConfig> UpdateConfigAsync(ParentalFilterConfigPatch request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Put, "parental/config/", request,
			ServicesJsonSerializerContext.Default.ParentalFilterConfigPatch,
			ServicesJsonSerializerContext.Default.ParentalFilterConfig, cancellationToken);
	}

	public FreeboxServiceCommand<ParentalFilterConfigPatch, ParentalFilterConfig> Configure() =>
		new(new ParentalFilterConfigPatch(), UpdateConfigAsync);

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#get--api-v4-parental-filter-" />
	public Task<ParentalFilter[]> GetFiltersAsync(CancellationToken cancellationToken = default)
	{
		return transport.SendAsync(HttpMethod.Get, "parental/filter/",
			ServicesJsonSerializerContext.Default.ParentalFilterArray, cancellationToken);
	}

	/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#post--api-v4-parental-filter-" />
	public Task<ParentalFilter> CreateFilterAsync(ParentalFilterWriteRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		request.Validate();
		return transport.SendAsync(HttpMethod.Post, "parental/filter/", request,
			ServicesJsonSerializerContext.Default.ParentalFilterWriteRequest,
			ServicesJsonSerializerContext.Default.ParentalFilter, cancellationToken);
	}

	public FreeboxServiceCommand<ParentalFilterWriteRequest, ParentalFilter> CreateFilter() =>
		new(new ParentalFilterWriteRequest(), CreateFilterAsync);

	public FreeboxParentalFilter Filter(long id) => new(transport, id);
}
