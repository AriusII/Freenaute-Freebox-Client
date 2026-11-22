namespace Freenaute.Freebox.AotSmoke;

internal sealed class DiscoveryFixtureHandler(bool fail = false) : HttpMessageHandler
{
	public int RequestCount { get; private set; }

	public static DiscoveryFixtureHandler FailingIfCalled() => new(fail: true);

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		RequestCount++;
		if (fail)
		{
			throw new InvalidOperationException("HTTP discovery should not run after a valid mDNS result.");
		}

		const string json = """
		                    {"uid":"fixture-http-uid","device_name":"Server","box_model":"fixture","box_model_name":"Fixture",
		                     "api_version":"4.0","api_domain":"fixture.example","api_base_url":"/api/",
		                     "https_available":true,"https_port":443}
		                    """;
		return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(json, Encoding.UTF8, "application/json")
		});
	}
}
