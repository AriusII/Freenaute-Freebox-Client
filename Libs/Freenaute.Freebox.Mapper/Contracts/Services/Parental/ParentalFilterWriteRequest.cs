namespace Freenaute.Freebox.Mapper.Contracts.Services.Parental;

/// <summary>
///     Write projection of ParentalFilter for create (POST) and update (PUT); unset fields are omitted and explicit
///     null is rejected. Read-only fields (id, hosts, ip, scheduling_mode, filter_state, ...) are not writable.
/// </summary>
/// <seealso href="https://dev.freebox.fr/sdk/os/parental/#ParentalFilter" />
public sealed record ParentalFilterWriteRequest
{
	[JsonPropertyName("macs")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string[]>))]
	public Optional<string[]> Macs { get; init; }

	[JsonPropertyName("desc")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<string>))]
	public Optional<string> Desc { get; init; }

	[JsonPropertyName("forced")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<bool>))]
	public Optional<bool> Forced { get; init; }

	[JsonPropertyName("forced_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ParentalFilterState>))]
	public Optional<ParentalFilterState> ForcedMode { get; init; }

	[JsonPropertyName("tmp_mode_expire")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<long>))]
	public Optional<long> TmpModeExpire { get; init; }

	[JsonPropertyName("tmp_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonConverter(typeof(OptionalJsonConverter<ParentalFilterState>))]
	public Optional<ParentalFilterState> TmpMode { get; init; }

	public void Validate()
	{
		if (Macs.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.Macs.", nameof(Macs));
		}

		if (Desc.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.Desc.", nameof(Desc));
		}

		if (Forced.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.Forced.", nameof(Forced));
		}

		if (ForcedMode.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.ForcedMode.",
				nameof(ForcedMode));
		}

		if (TmpModeExpire.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.TmpModeExpire.",
				nameof(TmpModeExpire));
		}

		if (TmpMode.IsNull)
		{
			throw new ArgumentException("Explicit null is not documented for ParentalFilter.TmpMode.", nameof(TmpMode));
		}
	}

	public override string ToString() => nameof(ParentalFilterWriteRequest);
}
