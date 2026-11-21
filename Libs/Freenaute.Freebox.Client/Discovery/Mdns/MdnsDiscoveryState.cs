namespace Freenaute.Freebox.Client.Discovery.Mdns;

internal sealed class MdnsDiscoveryState
{
	private static readonly StringComparer NameComparer = StringComparer.OrdinalIgnoreCase;
	private readonly Dictionary<string, HashSet<IPAddress>> _addresses = new(NameComparer);
	private readonly Dictionary<string, HashSet<string>> _instances = new(NameComparer);
	private readonly HashSet<string> _queried = new(NameComparer);
	private readonly Dictionary<string, MdnsRecord> _services = new(NameComparer);
	private readonly Dictionary<string, byte[]> _texts = new(NameComparer);

	internal void Add(IReadOnlyList<MdnsRecord> records)
	{
		foreach (var record in records)
		{
			if (record.TimeToLive == 0)
			{
				continue;
			}

			switch (record.Type)
			{
				case MdnsRecordType.Pointer when NameComparer.Equals(record.Name, MdnsWire.ServiceName) &&
				                                 record.Target is not null:
					if (!_instances.TryGetValue(record.Name, out var instances))
					{
						instances = new HashSet<string>(NameComparer);
						_instances[record.Name] = instances;
					}

					instances.Add(record.Target);
					break;
				case MdnsRecordType.Service when record.Target is not null && record.Port is > 0:
					_services[record.Name] = record;
					break;
				case MdnsRecordType.Text:
					if (record.Text is { Length: <= 4_096 })
					{
						_texts[record.Name] = record.Text;
					}

					break;
				case MdnsRecordType.Address or MdnsRecordType.AddressV6 when record.Address is not null:
					if (!_addresses.TryGetValue(record.Name, out var addresses))
					{
						addresses = [];
						_addresses[record.Name] = addresses;
					}

					addresses.Add(record.Address);
					break;
			}
		}
	}

	internal List<MdnsQuestion> GetMissingQuestions()
	{
		List<MdnsQuestion> questions = [];
		if (!_instances.TryGetValue(MdnsWire.ServiceName, out var instances))
		{
			return questions;
		}

		foreach (string instance in instances)
		{
			AddMissing(questions, instance, MdnsRecordType.Service, !_services.ContainsKey(instance));
			AddMissing(questions, instance, MdnsRecordType.Text, !_texts.ContainsKey(instance));
			if (_services.TryGetValue(instance, out var service) && service.Target is not null)
			{
				AddMissing(questions, service.Target, MdnsRecordType.Address,
					!_addresses.ContainsKey(service.Target));
				AddMissing(questions, service.Target, MdnsRecordType.AddressV6,
					!_addresses.ContainsKey(service.Target));
			}
		}

		return questions;
	}

	internal FreeboxDiscoveryResult? TryCreateResult()
	{
		if (!_instances.TryGetValue(MdnsWire.ServiceName, out var instances))
		{
			return null;
		}

		List<FreeboxDiscoveryResult> candidates = [];
		foreach (string instance in instances)
		{
			if (!_services.TryGetValue(instance, out var service) || service.Target is null ||
			    !_addresses.ContainsKey(service.Target) || !_texts.TryGetValue(instance, out byte[]? text))
			{
				continue;
			}

			if (TryParseText(text, out var values) && TryCreateResult(values, service.Port!.Value, out var candidate))
			{
				candidates.Add(candidate!);
			}
		}

		if (candidates.Count is 0)
		{
			return null;
		}

		string firstUid = candidates[0].ApiVersion.Uid!;
		if (candidates.Any(candidate => !NameComparer.Equals(candidate.ApiVersion.Uid, firstUid)) ||
		    candidates.Skip(1).Any(candidate => candidate != candidates[0]))
		{
			return null;
		}

		return candidates[0];
	}

	private void AddMissing(List<MdnsQuestion> questions, string name, MdnsRecordType type, bool isMissing)
	{
		string key = $"{name}|{(ushort)type}";
		if (isMissing && _queried.Add(key))
		{
			questions.Add(new MdnsQuestion(name, type));
		}
	}

	private static bool TryParseText(byte[] text, out Dictionary<string, string> values)
	{
		values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		int offset = 0;
		try
		{
			while (offset < text.Length)
			{
				int length = text[offset++];
				if (length is 0 || offset > text.Length - length)
				{
					return false;
				}

				string entry = new UTF8Encoding(false, true).GetString(text, offset, length);
				offset += length;
				int separator = entry.IndexOf('=', StringComparison.OrdinalIgnoreCase);
				if (separator <= 0 || !values.TryAdd(entry[..separator], entry[(separator + 1)..]))
				{
					return false;
				}
			}

			return true;
		}
		catch (DecoderFallbackException)
		{
			return false;
		}
	}

	private static bool TryCreateResult(Dictionary<string, string> values, int servicePort,
		out FreeboxDiscoveryResult? result)
	{
		result = null;
		if (!values.TryGetValue("uid", out string? uid) || !IsSafe(uid, 128) ||
		    !values.TryGetValue("api_version", out string? version) ||
		    !values.TryGetValue("api_base_url", out string? basePath) ||
		    !values.TryGetValue("api_domain", out string? domain) || !IsHost(domain) ||
		    !values.TryGetValue("https_available", out string? httpsText) ||
		    !bool.TryParse(httpsText, out bool httpsAvailable) || !httpsAvailable ||
		    !values.TryGetValue("https_port", out string? portText) ||
		    !int.TryParse(portText, NumberStyles.None, CultureInfo.InvariantCulture, out int port) ||
		    port is < 1 or > 65_535 || port != servicePort)
		{
			return false;
		}

		if (!TryParseMajor(version, out _) || !TryNormalizeBasePath(basePath, out string normalizedBasePath))
		{
			return false;
		}

		string? deviceName = values.GetValueOrDefault("device_name");
		string? boxModel = values.GetValueOrDefault("box_model");
		string? boxModelName = values.GetValueOrDefault("box_model_name");
		if ((deviceName is not null && !IsSafe(deviceName, 256)) ||
		    (boxModel is not null && !IsSafe(boxModel, 256)) ||
		    (boxModelName is not null && !IsSafe(boxModelName, 256)))
		{
			return false;
		}

		var metadata = new ApiVersionResponse(uid, deviceName, boxModel, boxModelName, version, domain,
			normalizedBasePath, true, port);
		var origin = new UriBuilder(Uri.UriSchemeHttps, domain, port).Uri;
		result = new FreeboxDiscoveryResult(origin, metadata);
		return true;
	}

	private static bool TryParseMajor(string version, out int major)
	{
		major = 0;
		if (string.IsNullOrWhiteSpace(version) || version.Length > 32)
		{
			return false;
		}

		int separator = version.IndexOf('.', StringComparison.OrdinalIgnoreCase);
		var majorSpan = separator < 0 ? version : version.AsSpan(0, separator);
		return int.TryParse(majorSpan, NumberStyles.None, CultureInfo.InvariantCulture, out major) && major > 0;
	}

	private static bool TryNormalizeBasePath(string path, out string normalized)
	{
		normalized = string.Empty;
		if (string.IsNullOrWhiteSpace(path) || path.Length > 512)
		{
			return false;
		}

		try
		{
			normalized = FreeboxApiPath.NormalizeBasePath(path);
			return true;
		}
		catch (ArgumentException)
		{
			return false;
		}
	}

	private static bool IsHost(string value) => !string.IsNullOrWhiteSpace(value) && value.Length <= 253 &&
	                                            !value.Any(char.IsControl) &&
	                                            Uri.CheckHostName(value) is not UriHostNameType.Unknown;

	private static bool IsSafe(string value, int maximumLength) => !string.IsNullOrWhiteSpace(value) &&
	                                                               value.Length <= maximumLength &&
	                                                               !value.Any(char.IsControl);
}
