namespace Freenaute.Freebox.Client.Domains.Services.Parental;

/// <summary>Parental control: global filter config, filter rules, and per-rule plannings.</summary>
public interface IFreeboxParentalApi
{
	Task<ParentalFilterConfig> GetConfigAsync(CancellationToken cancellationToken = default);

	Task<ParentalFilterConfig> UpdateConfigAsync(ParentalFilterConfigPatch request,
		CancellationToken cancellationToken = default);

	FreeboxServiceCommand<ParentalFilterConfigPatch, ParentalFilterConfig> Configure();

	Task<ParentalFilter[]> GetFiltersAsync(CancellationToken cancellationToken = default);

	Task<ParentalFilter> CreateFilterAsync(ParentalFilterWriteRequest request,
		CancellationToken cancellationToken = default);

	FreeboxServiceCommand<ParentalFilterWriteRequest, ParentalFilter> CreateFilter();

	FreeboxParentalFilter Filter(long id);
}
