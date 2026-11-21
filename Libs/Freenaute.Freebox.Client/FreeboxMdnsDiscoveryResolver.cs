namespace Freenaute.Freebox.Client;

internal sealed class FreeboxMdnsDiscoveryResolver : IFreeboxDiscoveryResolver
{
	private static readonly IPEndPoint MulticastV4 = new(IPAddress.Parse("224.0.0.251"), 5353);
	private static readonly IPEndPoint MulticastV6 = new(IPAddress.Parse("ff02::fb"), 5353);

	public async ValueTask<FreeboxDiscoveryResult?> DiscoverAsync(CancellationToken cancellationToken = default)
	{
		List<MdnsClient> clients;
		try
		{
			clients = CreateClients();
		}
		catch (NetworkInformationException)
		{
			return null;
		}
		catch (PlatformNotSupportedException)
		{
			return null;
		}

		if (clients.Count is 0)
		{
			return null;
		}

		try
		{
			var state = new MdnsDiscoveryState();
			await SendQueryAsync(clients, MdnsWire.CreateQuery([
				new MdnsQuestion(MdnsWire.ServiceName,
					MdnsRecordType.Pointer)
			]), cancellationToken).ConfigureAwait(false);

			var receives = clients.Select(client =>
			{
				client.ReceiveTask = ReceiveAsync(client, cancellationToken);
				return client.ReceiveTask;
			}).ToList();
			while (receives.Count > 0)
			{
				var completed = await Task.WhenAny(receives).ConfigureAwait(false);
				receives.Remove(completed);
				try
				{
					var datagram = await completed.ConfigureAwait(false);
					if (!MdnsWire.TryParseRecords(datagram.Buffer, out var records))
					{
						continue;
					}

					state.Add(records);
					var result = state.TryCreateResult();
					if (result is not null)
					{
						return result;
					}

					var questions = state.GetMissingQuestions();
					if (questions.Count > 0)
					{
						await SendQueryAsync(clients, MdnsWire.CreateQuery(questions), cancellationToken)
							.ConfigureAwait(false);
					}
				}
				catch (SocketException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
				finally
				{
					MdnsClient? client = null;
					foreach (var candidate in clients)
					{
						if (ReferenceEquals(candidate.ReceiveTask, completed))
						{
							client = candidate;
							break;
						}
					}

					if (client is not null && !cancellationToken.IsCancellationRequested)
					{
						client.ReceiveTask = ReceiveAsync(client, cancellationToken);
						receives.Add(client.ReceiveTask);
					}
				}
			}

			return null;
		}
		finally
		{
			foreach (var client in clients)
			{
				client.Dispose();
			}
		}
	}

	private static List<MdnsClient> CreateClients()
	{
		List<MdnsClient> clients = [];
		var interfaces = NetworkInterface.GetAllNetworkInterfaces()
			.Where(networkInterface => networkInterface.OperationalStatus is OperationalStatus.Up &&
			                           networkInterface.SupportsMulticast);
		foreach (var networkInterface in interfaces)
		{
			foreach (var unicast in networkInterface.GetIPProperties().UnicastAddresses)
			{
				switch (unicast.Address.AddressFamily)
				{
					case AddressFamily.InterNetwork when !IPAddress.IsLoopback(unicast.Address):
						AddClient(clients, AddressFamily.InterNetwork, unicast.Address,
							networkInterface.GetIPProperties().GetIPv4Properties()?.Index ?? 0);
						break;
					case AddressFamily.InterNetworkV6 when unicast.Address.IsIPv6LinkLocal:
						AddClient(clients, AddressFamily.InterNetworkV6, unicast.Address,
							(int)unicast.Address.ScopeId);
						break;
				}
			}
		}

		return clients;
	}

	private static void AddClient(List<MdnsClient> clients, AddressFamily family, IPAddress address, int interfaceIndex)
	{
		UdpClient? udp = null;
		try
		{
			udp = new UdpClient(family);
			udp.Client.ExclusiveAddressUse = false;
			udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			udp.Client.MulticastLoopback = true;
			udp.Client.Bind(new IPEndPoint(family is AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any,
				5353));
			if (family is AddressFamily.InterNetwork)
			{
				udp.JoinMulticastGroup(MulticastV4.Address, address);
			}
			else
			{
				if (interfaceIndex <= 0)
				{
					return;
				}

				udp.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership,
					new IPv6MulticastOption(MulticastV6.Address, interfaceIndex));
			}

			clients.Add(new MdnsClient(udp, family, address, interfaceIndex));
			udp = null;
		}
		catch (SocketException)
		{
			// Multicast may be blocked, or another responder may own the port exclusively.
		}
		finally
		{
			udp?.Dispose();
		}
	}

	private static Task<UdpReceiveResult> ReceiveAsync(MdnsClient client, CancellationToken cancellationToken) =>
		client.Client.ReceiveAsync(cancellationToken).AsTask();

	private static async Task SendQueryAsync(List<MdnsClient> clients, byte[] query,
		CancellationToken cancellationToken)
	{
		foreach (var client in clients)
		{
			try
			{
				if (client.Family is AddressFamily.InterNetwork)
				{
					client.Client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface,
						client.Address.GetAddressBytes());
					await client.Client.SendAsync(query, MulticastV4, cancellationToken).ConfigureAwait(false);
				}
				else
				{
					client.Client.Client.SetSocketOption(SocketOptionLevel.IPv6,
						SocketOptionName.MulticastInterface, client.InterfaceIndex);
					var endpoint = new IPEndPoint(
						new IPAddress(MulticastV6.Address.GetAddressBytes(), client.InterfaceIndex), 5353);
					await client.Client.SendAsync(query, endpoint, cancellationToken).ConfigureAwait(false);
				}
			}
			catch (SocketException)
			{
				// One unavailable interface must not prevent discovery on the others.
			}
		}
	}

	private sealed class MdnsClient(
		UdpClient client,
		AddressFamily family,
		IPAddress address,
		int interfaceIndex) : IDisposable
	{
		internal UdpClient Client { get; } = client;
		internal AddressFamily Family { get; } = family;
		internal IPAddress Address { get; } = address;
		internal int InterfaceIndex { get; } = interfaceIndex;
		internal Task<UdpReceiveResult>? ReceiveTask { get; set; }

		public void Dispose() => Client.Dispose();
	}
}
