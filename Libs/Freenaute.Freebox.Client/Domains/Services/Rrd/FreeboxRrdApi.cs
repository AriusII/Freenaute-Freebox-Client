using Freenaute.Freebox.Client.Domains.Services.Support;

namespace Freenaute.Freebox.Client.Domains.Services.Rrd;

/// <summary>RRD statistics routes.</summary>
public sealed class FreeboxRrdApi(IFreeboxTransport transport)
{
	private static JsonTypeInfo<JsonElement> JsonElementType => ServicesJsonSerializerContext.Default.JsonElement;

	/// <summary>Get RRD descriptors.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<JsonElement> GetAsync(CancellationToken cancellationToken = default) =>
		await transport.SendAsync(HttpMethod.Get, ServicesConstants.Routes.Rrd.Base, JsonElementType, cancellationToken).ConfigureAwait(false);

	/// <summary>Fetch RRD metrics using the documented GET route.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<RRDStats> GetAsync(RRDFetch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		Validate(request);
		return await transport.SendAsync(HttpMethod.Get, BuildQuery(request),
			NetworkJsonSerializerContext.Default.RRDStats,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>Fetch RRD metrics.</summary>
	/// <remarks><b>UNSTABLE API:</b> this route is marked unstable in the Freebox API documentation.</remarks>
	public async Task<RRDStats> FetchAsync(RRDFetch request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		Validate(request);
		return await transport.SendAsync(HttpMethod.Post, ServicesConstants.Routes.Rrd.Base, request,
			NetworkJsonSerializerContext.Default.RRDFetch,
			NetworkJsonSerializerContext.Default.RRDStats,
			cancellationToken).ConfigureAwait(false);
	}

	private static void Validate(RRDFetch request)
	{
		if (request.Db is not "net" and not "temp" and not "dsl" and not "switch")
		{
			throw new ArgumentException("Db must be one of: net, temp, dsl, switch.", nameof(request));
		}

		if (request.DateStart is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(request), "DateStart cannot be negative.");
		}

		if (request.DateEnd is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(request), "DateEnd cannot be negative.");
		}

		if (request.Precision is < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(request), "Precision cannot be negative.");
		}

		if (request.Fields is not null)
		{
			foreach (string? field in request.Fields)
			{
				if (string.IsNullOrWhiteSpace(field))
				{
					throw new ArgumentException("Fields cannot contain null or whitespace entries.", nameof(request));
				}
			}
		}
	}

	private static string BuildQuery(RRDFetch request)
	{
		List<string> values =
		[
			"db=" + Uri.EscapeDataString(request.Db)
		];
		if (request.DateStart is not null)
		{
			values.Add("date_start=" +
			           Uri.EscapeDataString(request.DateStart.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (request.DateEnd is not null)
		{
			values.Add("date_end=" +
			           Uri.EscapeDataString(request.DateEnd.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (request.Precision is not null)
		{
			values.Add("precision=" +
			           Uri.EscapeDataString(request.Precision.Value.ToString(CultureInfo.InvariantCulture)));
		}

		if (request.Fields is not null)
		{
			foreach (string field in request.Fields)
			{
				values.Add("fields=" + Uri.EscapeDataString(field));
			}
		}

		return "rrd/?" + string.Join('&', values);
	}
}
