namespace Freenaute.Freebox.Client.Http.Transport.Binary;

/// <summary>Streams raw responses (file downloads) over the shared HTTP transport core.</summary>
internal sealed class FreeboxBinaryTransport(FreeboxHttpTransportCore core) : IFreeboxBinaryTransport
{
	public async Task<FreeboxDownload> DownloadAsync(HttpMethod method, string relativePath,
		CancellationToken cancellationToken = default, bool requiresAuthentication = true)
	{
		ArgumentNullException.ThrowIfNull(method);
		FreeboxApiPath.ValidateRelativePath(relativePath);
		cancellationToken.ThrowIfCancellationRequested();
		var lifetime = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		var timeout = core.EffectiveTimeout;
		if (timeout != Timeout.InfiniteTimeSpan)
		{
			lifetime.CancelAfter(timeout);
		}

		HttpResponseMessage? response = null;
		SessionResponse? session = null;
		try
		{
			var apiAddress = await core.GetApiAddressAsync(lifetime.Token).ConfigureAwait(false);
			session = await core.ResolveSessionAsync(requiresAuthentication,
				"Open a session before making authenticated requests, or enable automatic authentication.",
				lifetime.Token).ConfigureAwait(false);
			using var request = new HttpRequestMessage(method, FreeboxApiPath.Resolve(apiAddress, relativePath));
			if (session is not null)
			{
				request.Headers.Add(FreeboxHttpTransportCore.AuthenticationHeader, session.SessionToken);
			}

			response = await core.HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
				lifetime.Token).ConfigureAwait(false);
			if (!response.IsSuccessStatusCode)
			{
				var errorBody = await response.Content.ReadAsStreamAsync(lifetime.Token).ConfigureAwait(false);
				await using var errorBodyLifetime = errorBody.ConfigureAwait(false);
				try
				{
					using var error = await JsonDocument.ParseAsync(errorBody, cancellationToken: lifetime.Token)
						.ConfigureAwait(false);
					throw FreeboxHttpTransportCore.CreateApiException(response.StatusCode, error.RootElement);
				}
				catch (JsonException)
				{
					throw new FreeboxApiException(response.StatusCode);
				}
			}

			var stream = await response.Content.ReadAsStreamAsync(lifetime.Token).ConfigureAwait(false);
			var download = new FreeboxDownload(response, stream, lifetime);
			response = null;
			return download;
		}
		catch (FreeboxApiException exception) when (session is not null &&
		                                            (exception.StatusCode is HttpStatusCode.Unauthorized ||
		                                             exception.ErrorCode is "auth_required" or "invalid_session"))
		{
			core.InvalidateSession(session);
			response?.Dispose();
			lifetime.Dispose();
			throw;
		}
		catch
		{
			response?.Dispose();
			lifetime.Dispose();
			throw;
		}
	}
}
