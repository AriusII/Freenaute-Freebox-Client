namespace Freenaute.Freebox.Client.Domains.Services;

public sealed class FreeboxPvrApi(IFreeboxTransport transport)
{
    public Task<PvrConfig> GetConfigurationAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "pvr/config/", ServicesJsonSerializerContext.Default.PvrConfig,
            cancellationToken);
    }

    /// <summary>
    ///     Updates documented configuration fields and verifies the common success envelope; no undocumented result shape
    ///     is assumed.
    /// </summary>
    public Task UpdateConfigurationAsync(PvrConfigPatch request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        request.Validate();
        return transport.SendAsync(HttpMethod.Put, "pvr/config/", request,
            ServicesJsonSerializerContext.Default.PvrConfigPatch, cancellationToken);
    }

    public FreeboxServiceCommand<PvrConfigPatch> Configure()
    {
        return new FreeboxServiceCommand<PvrConfigPatch>(new PvrConfigPatch(), UpdateConfigurationAsync);
    }

    public Task<PvrQuota> GetQuotaAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "pvr/quota/", ServicesJsonSerializerContext.Default.PvrQuota,
            cancellationToken);
    }

    public Task<PvrQuota> RecalculateQuotaAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Put, "pvr/quota/", new PvrQuotaRecalculationRequest(),
            ServicesJsonSerializerContext.Default.PvrQuotaRecalculationRequest,
            ServicesJsonSerializerContext.Default.PvrQuota, cancellationToken);
    }

    public Task<ProgrammedRecording[]> GetProgrammedAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "pvr/programmed/",
            ServicesJsonSerializerContext.Default.ProgrammedRecordingArray, cancellationToken);
    }

    public FreeboxServiceResource<ProgrammedRecording, ProgrammedRecordingWriteRequest> Programmed(string id)
    {
        return new FreeboxServiceResource<ProgrammedRecording, ProgrammedRecordingWriteRequest>(transport,
            $"pvr/programmed/{ServicesPath.Segment(id)}",
            ServicesJsonSerializerContext.Default.ProgrammedRecording,
            ServicesJsonSerializerContext.Default.ProgrammedRecordingWriteRequest, request => request.Validate());
    }

    public FreeboxServiceResource<ProgrammedRecording, ProgrammedRecordingWriteRequest> Programmed(long id)
    {
        return Programmed(ServicesPath.Id(id));
    }

    public Task<ProgrammedRecording> CreateProgrammedAsync(ProgrammedRecordingWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        request.Validate();
        return transport.SendAsync(HttpMethod.Post, "pvr/programmed/", request,
            ServicesJsonSerializerContext.Default.ProgrammedRecordingWriteRequest,
            ServicesJsonSerializerContext.Default.ProgrammedRecording, cancellationToken);
    }

    public FreeboxServiceCommand<ProgrammedRecordingWriteRequest, ProgrammedRecording> CreateProgrammed()
    {
        return new FreeboxServiceCommand<ProgrammedRecordingWriteRequest, ProgrammedRecording>(
            new ProgrammedRecordingWriteRequest(), CreateProgrammedAsync);
    }

    public Task<FinishedRecording[]> GetFinishedAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "pvr/finished/",
            ServicesJsonSerializerContext.Default.FinishedRecordingArray, cancellationToken);
    }

    public FreeboxServiceResource<FinishedRecording, FinishedRecordingPatch> Finished(string id)
    {
        return new FreeboxServiceResource<FinishedRecording, FinishedRecordingPatch>(transport,
            $"pvr/finished/{ServicesPath.Segment(id)}",
            ServicesJsonSerializerContext.Default.FinishedRecording,
            ServicesJsonSerializerContext.Default.FinishedRecordingPatch, request => request.Validate());
    }

    public FreeboxServiceResource<FinishedRecording, FinishedRecordingPatch> Finished(long id)
    {
        return Finished(ServicesPath.Id(id));
    }

    public Task<PvrMedia[]> GetMediaAsync(CancellationToken cancellationToken = default)
    {
        return transport.SendAsync(HttpMethod.Get, "pvr/media/", ServicesJsonSerializerContext.Default.PvrMediaArray,
            cancellationToken);
    }
}
