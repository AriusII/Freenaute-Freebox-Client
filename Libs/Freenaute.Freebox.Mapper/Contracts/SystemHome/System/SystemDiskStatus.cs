namespace Freenaute.Freebox.Mapper.Contracts.SystemHome.System;

/// <summary>
///     Wire token from http://mafreebox.freebox.fr/doc/index.html#SystemConfig.disk_status; unknown values are
///     retained.
/// </summary>
[JsonConverter(typeof(SystemHomeWireTokenJsonConverter<SystemDiskStatus>))]
public readonly record struct SystemDiskStatus(string Value) : ISystemHomeWireToken<SystemDiskStatus>
{
	public bool IsKnown => Value is "not_detected" or "disabled" or "initializing" or "error" or "active";
	public static SystemDiskStatus NotDetected { get; } = new("not_detected");
	public static SystemDiskStatus Disabled { get; } = new("disabled");
	public static SystemDiskStatus Initializing { get; } = new("initializing");
	public static SystemDiskStatus Error { get; } = new("error");
	public static SystemDiskStatus Active { get; } = new("active");

	public static SystemDiskStatus FromWire(string value)
	{
		ArgumentNullException.ThrowIfNull(value);
		return new SystemDiskStatus(value);
	}

	public override string ToString() => Value ?? string.Empty;
}
