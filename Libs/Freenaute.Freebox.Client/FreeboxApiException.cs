namespace Freenaute.Freebox.Client;

/// <summary>An HTTP failure or a Freebox envelope with <c>success: false</c>.</summary>
public sealed class FreeboxApiException : Exception
{
    internal FreeboxApiException(HttpStatusCode statusCode, string? errorCode = null,
        string? apiMessage = null, string? errorUid = null, JsonElement? details = null)
        : base($"The Freebox request failed (HTTP {(int)statusCode}).")
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ApiMessage = apiMessage;
        ErrorUid = errorUid;
        Details = details;
    }

    public HttpStatusCode? StatusCode { get; }
    public string? ErrorCode { get; }
    public string? ApiMessage { get; }
    public string? ErrorUid { get; }
    public JsonElement? Details { get; }
}
