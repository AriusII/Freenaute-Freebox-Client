namespace Freenaute.Freebox.Mapper.Contracts.SystemHome;

/// <summary>Read contract from http://mafreebox.freebox.fr/doc/index.html#SystemConfig; absent metadata remains nullable.</summary>
public sealed record SystemConfig
{
	[JsonPropertyName("firmware_version")] public string? FirmwareVersion { get; init; }
	[JsonPropertyName("mac")] public string? Mac { get; init; }
	[JsonPropertyName("serial")] public string? Serial { get; init; }
	[JsonPropertyName("uptime")] public string? Uptime { get; init; }
	[JsonPropertyName("uptime_val")] public long? UptimeVal { get; init; }
	[JsonPropertyName("board_name")] public string? BoardName { get; init; }

	[JsonPropertyName("box_authenticated")]
	public bool? BoxAuthenticated { get; init; }

	[JsonPropertyName("disk_status")] public SystemDiskStatus? DiskStatus { get; init; }
	[JsonPropertyName("usb3_enable")] public bool? Usb3Enable { get; init; }

	[JsonPropertyName("user_main_storage")]
	public string? UserMainStorage { get; init; }

	[JsonPropertyName("user_storage_powered")]
	public bool? UserStoragePowered { get; init; }

	[JsonPropertyName("sensors")] public ImmutableArray<SystemConfigSensor>? Sensors { get; init; }
	[JsonPropertyName("model_info")] public SystemModelInfo? ModelInfo { get; init; }
	[JsonPropertyName("fans")] public ImmutableArray<SystemConfigFan>? Fans { get; init; }
	[JsonPropertyName("expansions")] public ImmutableArray<SystemConfigExpansion>? Expansions { get; init; }
}
