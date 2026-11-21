namespace Freenaute.Freebox.Client.Domains.Services.Parental;

/// <summary>A single parental filter rule with its planning sub-resource.</summary>
public sealed class FreeboxParentalFilter
{
	private readonly FreeboxServiceResource<ParentalFilter, ParentalFilterWriteRequest> _resource;

	internal FreeboxParentalFilter(IFreeboxTransport transport, long id)
	{
		string path = $"parental/filter/{ServicesPath.Id(id)}";
		_resource = new FreeboxServiceResource<ParentalFilter, ParentalFilterWriteRequest>(transport, path,
			ServicesJsonSerializerContext.Default.ParentalFilter,
			ServicesJsonSerializerContext.Default.ParentalFilterWriteRequest, request => request.Validate());
		Planning = new FreeboxServiceResource<ParentalFilterPlanning, ParentalFilterPlanningPatch>(transport,
			$"{path}/planning", ServicesJsonSerializerContext.Default.ParentalFilterPlanning,
			ServicesJsonSerializerContext.Default.ParentalFilterPlanningPatch, request => request.Validate());
	}

	/// <summary>The per-rule planning (GET/PUT <c>parental/filter/{id}/planning</c>).</summary>
	public FreeboxServiceResource<ParentalFilterPlanning, ParentalFilterPlanningPatch> Planning { get; }

	public Task<ParentalFilter> GetAsync(CancellationToken cancellationToken = default) =>
		_resource.GetAsync(cancellationToken);

	public Task<ParentalFilter> UpdateAsync(ParentalFilterWriteRequest request,
		CancellationToken cancellationToken = default) =>
		_resource.UpdateAsync(request, cancellationToken);

	public Task DeleteAsync(CancellationToken cancellationToken = default) =>
		_resource.DeleteAsync(cancellationToken);

	public FreeboxServiceCommand<ParentalFilterWriteRequest, ParentalFilter> Configure() =>
		_resource.Configure();
}
