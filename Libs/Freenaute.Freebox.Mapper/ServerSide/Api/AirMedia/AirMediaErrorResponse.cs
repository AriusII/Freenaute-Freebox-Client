namespace Freenaute.Freebox.Mapper.ServerSide.Api.AirMedia;

public sealed record AirMediaErrorResponse(
    [property: JsonPropertyName("success")]
    bool Success,
    [property: JsonPropertyName("error_code")]
    AirMediaErrorCodeEnum ErrorCode
);
