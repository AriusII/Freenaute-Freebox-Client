namespace Freenaute.Freebox.Client.Tests;

public sealed class TlsTests
{
	[Fact]
	public void DefaultOriginAndScopedTlsPolicyUseVerifiedSdkRoots()
	{
		Assert.Equal("https://mafreebox.freebox.fr/", new FreeboxClientOptions().ServerAddress.AbsoluteUri);
		using var handler = FreeboxTls.CreateHandler();
		var sockets = GetSockets(handler);
		var policy = Assert.IsType<X509ChainPolicy>(sockets.SslOptions.CertificateChainPolicy);

		Assert.False(sockets.AllowAutoRedirect);
		Assert.False(sockets.UseCookies);
		Assert.Null(sockets.SslOptions.RemoteCertificateValidationCallback);
		Assert.Equal(X509ChainTrustMode.CustomRootTrust, policy.TrustMode);
		Assert.Equal(X509VerificationFlags.NoFlag, policy.VerificationFlags);
		Assert.Equal(X509RevocationMode.NoCheck, policy.RevocationMode);
		Assert.Equal("1.3.6.1.5.5.7.3.1", Assert.Single(policy.ApplicationPolicy.Cast<Oid>()).Value);
		Assert.Equal(
			new[]
			{
				"23cfb72636be665dbbf2fe3e0527904771a8dff1bace461d8bd69f34812c2444",
				"2bd8b5be1a990e42ad1bd79c306eb519b637ee2475c0d931f257535610e9c3e7"
			}, policy.CustomTrustStore
				.Select(certificate => Convert.ToHexStringLower(SHA256.HashData(certificate.RawData))).Order()
				.ToArray());
	}

	[Fact]
	public void SeparateHandlersOwnSeparateCertificatesAndDisposeThem()
	{
		var first = FreeboxTls.CreateHandler();
		using var second = FreeboxTls.CreateHandler();
		var firstRoot = GetSockets(first).SslOptions.CertificateChainPolicy!.CustomTrustStore[0];
		var secondRoot = GetSockets(second).SslOptions.CertificateChainPolicy!.CustomTrustStore[0];
		Assert.NotSame(firstRoot, secondRoot);

		first.Dispose();
		first.Dispose();

		Assert.ThrowsAny<CryptographicException>(() => firstRoot.GetCertHash());
		Assert.NotEmpty(secondRoot.GetCertHash());
	}

	[Theory]
	[InlineData(X509RevocationMode.Online)]
	[InlineData(X509RevocationMode.Offline)]
	public void ExplicitRevocationPolicyIsRetained(X509RevocationMode mode)
	{
		using var handler = FreeboxTls.CreateHandler(mode);
		Assert.Equal(mode, GetSockets(handler).SslOptions.CertificateChainPolicy!.RevocationMode);
	}

	[Theory]
	[InlineData("valid", true)]
	[InlineData("wrong-name", false)]
	[InlineData("expired", false)]
	[InlineData("client-auth-only", false)]
	[InlineData("untrusted", false)]
	public async Task RealTlsHandshakeRequiresTrustedRootHostnameLifetimeAndServerEku(string scenario,
		bool expectedSuccess)
	{
		using var certificates = new TestCertificates(scenario);
		using var handler = FreeboxTls.CreateHandler();
		var sockets = GetSockets(handler);
		sockets.UseProxy = false;
		// Replace only this handler's roots with a synthetic CA whose private key the local server owns.
		// Production roots, the system store, and TLS name/EKU/validity checks are never changed.
		if (scenario is not "untrusted")
		{
			sockets.SslOptions.CertificateChainPolicy!.CustomTrustStore.Clear();
			sockets.SslOptions.CertificateChainPolicy.CustomTrustStore.Add(certificates.Root);
		}

		using var listener = new TcpListener(IPAddress.Loopback, 0);
		listener.Start();
		var endpoint = (IPEndPoint)listener.LocalEndpoint;
		using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
		var server = ServeOnceAsync(listener, certificates.Leaf, expectedSuccess, cancellation.Token);
		using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) };
		var address = new Uri(string.Create(CultureInfo.InvariantCulture, $"https://localhost:{endpoint.Port}/probe/"));

		if (expectedSuccess)
		{
			var response = http.GetStringAsync(address, cancellation.Token);
			await server.WaitAsync(TimeSpan.FromSeconds(10));
			Assert.Equal("TLS fixture", await response);
			return;
		}

		var error = await Assert.ThrowsAsync<HttpRequestException>(() =>
			http.GetStringAsync(address, cancellation.Token));
		Assert.Equal(HttpRequestError.SecureConnectionError, error.HttpRequestError);

		await server.WaitAsync(TimeSpan.FromSeconds(10));
	}

	private static SocketsHttpHandler GetSockets(HttpMessageHandler handler) =>
		Assert.IsType<SocketsHttpHandler>(Assert.IsAssignableFrom<DelegatingHandler>(handler).InnerHandler);

	private static async Task ServeOnceAsync(TcpListener listener, X509Certificate2 certificate,
		bool expectedSuccess, CancellationToken cancellationToken)
	{
		using var client = await listener.AcceptTcpClientAsync(cancellationToken);
		await using var stream = new SslStream(client.GetStream());
		try
		{
			await stream.AuthenticateAsServerAsync(
				new SslServerAuthenticationOptions
				{
					ServerCertificate = certificate, EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
				}, cancellationToken);
			byte[] buffer = new byte[4096];
			int count = 0;
			while (count < buffer.Length)
			{
				int read = await stream.ReadAsync(buffer.AsMemory(count), cancellationToken);
				if (read is 0)
				{
					return; // A client can reject the certificate immediately after the handshake.
				}

				count += read;
				if (Encoding.ASCII.GetString(buffer, 0, count).Contains("\r\n\r\n", StringComparison.Ordinal))
				{
					break;
				}
			}

			await stream.WriteAsync(Encoding.ASCII.GetBytes(
				"HTTP/1.1 200 OK\r\nContent-Length: 11\r\nConnection: close\r\n\r\nTLS fixture"), cancellationToken);
		}
		catch (AuthenticationException)
		{
			/* Expected when the client rejects a certificate. */
		}
		catch (IOException)
		{
			/* A rejected TLS connection can close without a TLS alert. */
		}
	}

	private sealed class TestCertificates : IDisposable
	{
		public TestCertificates(string scenario)
		{
			var now = DateTimeOffset.UtcNow;
			using var rootKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
			var rootRequest = new CertificateRequest("CN=Freebox test synthetic CA", rootKey, HashAlgorithmName.SHA256);
			rootRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
			rootRequest.CertificateExtensions.Add(
				new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign, true));
			rootRequest.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(rootRequest.PublicKey, false));
			Root = rootRequest.CreateSelfSigned(now.AddYears(-1), now.AddYears(1));

			using var leafKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
			var leafRequest =
				new CertificateRequest("CN=Freebox test synthetic server", leafKey, HashAlgorithmName.SHA256);
			leafRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
			leafRequest.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, true));
			leafRequest.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
				new OidCollection
				{
					new Oid(scenario is "client-auth-only" ? "1.3.6.1.5.5.7.3.2" : "1.3.6.1.5.5.7.3.1")
				}, true));
			var san = new SubjectAlternativeNameBuilder();
			if (scenario is "wrong-name")
			{
				san.AddDnsName("other.example");
			}
			else
			{
				san.AddDnsName("localhost");
			}

			leafRequest.CertificateExtensions.Add(san.Build());
			using var issued = leafRequest.Create(Root, now.AddDays(-2),
				scenario is "expired" ? now.AddDays(-1) : now.AddDays(1), RandomNumberGenerator.GetBytes(16));
			using var issuedWithKey = issued.CopyWithPrivateKey(leafKey);
			byte[] pfx = issuedWithKey.Export(X509ContentType.Pfx, "fixture");
			Leaf = X509CertificateLoader.LoadPkcs12(pfx, "fixture",
				X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
		}

		public X509Certificate2 Root { get; }
		public X509Certificate2 Leaf { get; }

		public void Dispose()
		{
			Leaf.Dispose();
			Root.Dispose();
		}
	}
}
