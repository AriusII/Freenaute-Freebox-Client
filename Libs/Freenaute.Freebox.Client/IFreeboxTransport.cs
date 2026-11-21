namespace Freenaute.Freebox.Client;

/// <summary>A reflection-free transport. Response metadata describes the envelope's <c>result</c>.</summary>
public interface IFreeboxTransport
{
	/// <summary>
	///     Sends form or multipart content and reads a generated JSON result. Ownership of content transfers to this
	///     operation.
	/// </summary>
	Task<TResponse> SendContentAsync<TResponse>(HttpMethod method, string relativePath,
		HttpContent content, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true);

	/// <summary>Sends form or multipart content for a success-only command. Ownership of content transfers to this operation.</summary>
	Task SendContentAsync(HttpMethod method, string relativePath, HttpContent content,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true);

	Task<TResponse> SendAsync<TResponse>(HttpMethod method, string relativePath,
		JsonTypeInfo<TResponse> responseType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true);

	Task<TResponse> SendAsync<TRequest, TResponse>(HttpMethod method, string relativePath,
		TRequest request, JsonTypeInfo<TRequest> requestType, JsonTypeInfo<TResponse> responseType,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true);

	Task SendAsync(HttpMethod method, string relativePath, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true);

	Task SendAsync<TRequest>(HttpMethod method, string relativePath, TRequest request,
		JsonTypeInfo<TRequest> requestType, CancellationToken cancellationToken = default,
		bool requiresAuthentication = true);
}
