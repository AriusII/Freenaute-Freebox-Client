namespace Freenaute.Freebox.Mapper.ServerSide.Api.AirMedia;

public sealed record AirMediaConfigResponse(
    [property: JsonPropertyName("enabled")]
    bool Enabled
);
