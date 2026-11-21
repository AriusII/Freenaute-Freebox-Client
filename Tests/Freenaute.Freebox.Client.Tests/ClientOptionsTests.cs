namespace Freenaute.Freebox.Client.Tests;

public sealed class ClientOptionsTests
{
	[Fact]
	public void GeneratedValidatorRejectsInvalidOptions()
	{
		var validator = new FreeboxClientOptionsValidator();
		var options = new FreeboxClientOptions
		{
			ServerAddress = new Uri("https://fixture.example/path"),
			ApiVersion = 0,
			Timeout = TimeSpan.Zero,
			MdnsTimeout = TimeSpan.FromSeconds(31),
			HttpClientName = ""
		};

		var result = validator.Validate(null, options);

		Assert.True(result.Failed);
		Assert.Contains(result.Failures!, failure => failure.Contains("HTTP(S) origin", StringComparison.Ordinal));
		Assert.Contains(result.Failures!, failure => failure.Contains("major version", StringComparison.Ordinal));
		Assert.Contains(result.Failures!, failure => failure.Contains("Timeout", StringComparison.Ordinal));
		Assert.Contains(result.Failures!, failure => failure.Contains("MdnsTimeout", StringComparison.Ordinal));
		Assert.Contains(result.Failures!, failure => failure.Contains("HttpClientName", StringComparison.Ordinal));
	}

	[Fact]
	public void InvalidConfigurationValueFailsAsOptionsErrorWithoutFallback()
	{
		var values = new Dictionary<string, string?>
		{
			["ApiVersion"] = "not-an-integer", ["ApplicationId"] = "org.fixture"
		};
		IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();
		var services = new ServiceCollection();
		services.AddFreeboxClient(configuration);
		using var provider = services.BuildServiceProvider();

		var failure = Assert.ThrowsAny<Exception>(() =>
			provider.GetRequiredService<IOptions<FreeboxClientOptions>>().Value);

		Assert.Contains("ApiVersion", failure.ToString(), StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public void ConfigurationCannotInjectAppTokenIntoOptions()
	{
		IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(
				new Dictionary<string, string?>
				{
					["ApplicationId"] = "org.fixture", ["AppToken"] = "must-not-be-bound"
				})
			.Build();
		var services = new ServiceCollection();
		services.AddFreeboxClient(configuration);
		using var provider = services.BuildServiceProvider();

		var options = provider.GetRequiredService<IOptions<FreeboxClientOptions>>().Value;

		Assert.DoesNotContain("AppToken",
			typeof(FreeboxClientOptions).GetProperties().Select(property => property.Name));
		Assert.Equal("org.fixture", options.ApplicationId);
	}

	[Fact]
	public void DefaultAndValidateOnStartRegistrationsKeepOptionsOwnerSpecific()
	{
		var services = new ServiceCollection();
		services.AddFreeboxClient(options => options.ApiVersion = 16, true);
		services.AddFreeboxClient("home", options =>
		{
			options.ApiVersion = 17;
			options.CredentialProfile = "home";
		});
		using var provider = services.BuildServiceProvider();

		var options = provider.GetRequiredService<IOptions<FreeboxClientOptions>>().Value;
		var named = provider.GetRequiredService<IOptionsMonitor<FreeboxClientOptions>>().Get("home");

		Assert.Equal(16, options.ApiVersion);
		Assert.Equal(17, named.ApiVersion);
		Assert.Equal("home", named.CredentialProfile);
	}
}
