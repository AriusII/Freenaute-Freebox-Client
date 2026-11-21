namespace Freenaute.Freebox.Client.Discovery.Mdns;

internal static class MdnsWire
{
	internal const string ServiceName = "_fbx-api._tcp.local.";
	internal const int MaximumPacketLength = 9_000;
	private const int MaximumRecordCount = 128;

	internal static byte[] CreateQuery(IReadOnlyList<MdnsQuestion> questions)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
		writer.Write((short)0);
		writer.Write((short)0);
		writer.Write(ToNetwork((ushort)questions.Count));
		writer.Write((short)0);
		writer.Write((short)0);
		writer.Write((short)0);
		foreach (var question in questions)
		{
			WriteName(writer, question.Name);
			writer.Write(ToNetwork((ushort)question.Type));
			writer.Write(ToNetwork(0x8001));
		}

		return stream.ToArray();
	}

	internal static bool TryParseRecords(ReadOnlySpan<byte> packet, out IReadOnlyList<MdnsRecord> records)
	{
		var parsed = new List<MdnsRecord>();
		records = parsed;
		if (packet.Length is < 12 or > MaximumPacketLength)
		{
			return false;
		}

		int questionCount = ReadNetworkUInt16(packet, 4);
		int answerCount = ReadNetworkUInt16(packet, 6);
		int authorityCount = ReadNetworkUInt16(packet, 8);
		int additionalCount = ReadNetworkUInt16(packet, 10);
		if (questionCount > MaximumRecordCount || answerCount + authorityCount + additionalCount > MaximumRecordCount)
		{
			return false;
		}

		int offset = 12;
		try
		{
			for (int index = 0; index < questionCount; index++)
			{
				ReadName(packet, ref offset);
				RequireAvailable(packet, offset, 4);
				offset += 4;
			}

			int recordCount = answerCount + authorityCount + additionalCount;
			for (int index = 0; index < recordCount; index++)
			{
				string name = ReadName(packet, ref offset);
				RequireAvailable(packet, offset, 10);
				var type = (MdnsRecordType)ReadNetworkUInt16(packet, offset);
				offset += 4; // Type and class; the cache-flush bit is not part of the class.
				uint ttl = ReadNetworkUInt32(packet, offset);
				offset += 4;
				int length = ReadNetworkUInt16(packet, offset);
				offset += 2;
				RequireAvailable(packet, offset, length);
				if (!TryParseRecord(packet, name, type, ttl, offset, length, out var record))
				{
					offset += length;
					continue;
				}

				parsed.Add(record!);
				offset += length;
			}

			return true;
		}
		catch (FormatException)
		{
			parsed.Clear();
			return false;
		}
	}

	private static bool TryParseRecord(ReadOnlySpan<byte> packet, string name, MdnsRecordType type, uint ttl,
		int dataOffset, int dataLength, out MdnsRecord? record)
	{
		record = null;
		try
		{
			switch (type)
			{
				case MdnsRecordType.Pointer:
					int pointerOffset = dataOffset;
					record = new MdnsRecord(name, type, ttl, ReadName(packet, ref pointerOffset), null, null, null);
					return pointerOffset == dataOffset + dataLength;
				case MdnsRecordType.Service:
					if (dataLength < 7)
					{
						return false;
					}

					int targetOffset = dataOffset + 6;
					string target = ReadName(packet, ref targetOffset);
					record = new MdnsRecord(name, type, ttl, target, ReadNetworkUInt16(packet, dataOffset + 4),
						null, null);
					return targetOffset == dataOffset + dataLength;
				case MdnsRecordType.Text:
					if (dataLength > 4_096)
					{
						return false;
					}

					record = new MdnsRecord(name, type, ttl, null, null, packet.Slice(dataOffset, dataLength).ToArray(),
						null);
					return true;
				case MdnsRecordType.Address when dataLength is 4:
					record = new MdnsRecord(name, type, ttl, null, null, null,
						new IPAddress(packet.Slice(dataOffset, 4)));
					return true;
				case MdnsRecordType.AddressV6 when dataLength is 16:
					record = new MdnsRecord(name, type, ttl, null, null, null,
						new IPAddress(packet.Slice(dataOffset, 16)));
					return true;
				default:
					return false;
			}
		}
		catch (FormatException)
		{
			return false;
		}
	}

	internal static string ReadName(ReadOnlySpan<byte> packet, ref int offset)
	{
		var labels = new List<string>();
		int length = 0;
		int? resumeOffset = null;
		int jumps = 0;
		while (true)
		{
			RequireAvailable(packet, offset, 1);
			byte size = packet[offset++];
			if (size is 0)
			{
				offset = resumeOffset ?? offset;
				return labels.Count is 0 ? "." : string.Join(".", labels) + ".";
			}

			if ((size & 0xC0) is 0xC0)
			{
				RequireAvailable(packet, offset, 1);
				int pointer = ((size & 0x3F) << 8) | packet[offset++];
				if (pointer >= packet.Length || ++jumps > 16)
				{
					throw new FormatException("Invalid DNS compression pointer.");
				}

				resumeOffset ??= offset;
				offset = pointer;
				continue;
			}

			if ((size & 0xC0) is not 0 || size > 63)
			{
				throw new FormatException("Invalid DNS label length.");
			}

			RequireAvailable(packet, offset, size);
			length += size + 1;
			if (length > 255)
			{
				throw new FormatException("DNS name exceeds 255 bytes.");
			}

			labels.Add(Encoding.ASCII.GetString(packet.Slice(offset, size)));
			offset += size;
		}
	}

	private static void WriteName(BinaryWriter writer, string name)
	{
		foreach (string label in name.TrimEnd('.').Split('.'))
		{
			byte[] bytes = Encoding.ASCII.GetBytes(label);
			if (bytes.Length is 0 or > 63)
			{
				throw new ArgumentException("A DNS label must contain between 1 and 63 ASCII bytes.", nameof(name));
			}

			writer.Write((byte)bytes.Length);
			writer.Write(bytes);
		}

		writer.Write((byte)0);
	}

	private static void RequireAvailable(ReadOnlySpan<byte> packet, int offset, int length)
	{
		if (offset < 0 || length < 0 || offset > packet.Length - length)
		{
			throw new FormatException("The DNS message is truncated.");
		}
	}

	private static ushort ReadNetworkUInt16(ReadOnlySpan<byte> value, int offset)
	{
		RequireAvailable(value, offset, 2);
		return (ushort)((value[offset] << 8) | value[offset + 1]);
	}

	private static uint ReadNetworkUInt32(ReadOnlySpan<byte> value, int offset)
	{
		RequireAvailable(value, offset, 4);
		return ((uint)value[offset] << 24) | ((uint)value[offset + 1] << 16) |
		       ((uint)value[offset + 2] << 8) | value[offset + 3];
	}

	private static short ToNetwork(ushort value) => unchecked((short)((value >> 8) | (value << 8)));
}
