namespace Freenaute.Freebox.Mapper.Common.Types;

/// <summary>Classifies known discovery model names while preserving the server's original string.</summary>
internal static class BoxModelNames
{
	public static BoxModels? Classify(string? value)
	{
		return value switch
		{
			MapperConstants.BoxModels.FreeboxServerV6R1 => BoxModels.FreeboxServerV6R1,
			MapperConstants.BoxModels.FreeboxServerV6R2 => BoxModels.FreeboxServerV6R2,
			MapperConstants.BoxModels.FreeboxMiniR1 => BoxModels.FreeboxMiniR1,
			MapperConstants.BoxModels.FreeboxMiniR2 => BoxModels.FreeboxMiniR2,
			MapperConstants.BoxModels.FreeboxOneR1 => BoxModels.FreeboxOneR1,
			MapperConstants.BoxModels.FreeboxOneR2 => BoxModels.FreeboxOneR2,
			MapperConstants.BoxModels.FreeboxV7R1 => BoxModels.FreeboxV7R1,
			MapperConstants.BoxModels.FreeboxV8R1 => BoxModels.FreeboxV8R1,
			MapperConstants.BoxModels.FreeboxV9R1 or MapperConstants.BoxModels.FreeboxV9R1Compact => BoxModels
				.FreeboxV9R1,
			_ => null
		};
	}

	public static string GetWireName(this BoxModels value)
	{
		return value switch
		{
			BoxModels.FreeboxServerV6R1 => MapperConstants.BoxModels.FreeboxServerV6R1,
			BoxModels.FreeboxServerV6R2 => MapperConstants.BoxModels.FreeboxServerV6R2,
			BoxModels.FreeboxMiniR1 => MapperConstants.BoxModels.FreeboxMiniR1,
			BoxModels.FreeboxMiniR2 => MapperConstants.BoxModels.FreeboxMiniR2,
			BoxModels.FreeboxOneR1 => MapperConstants.BoxModels.FreeboxOneR1,
			BoxModels.FreeboxOneR2 => MapperConstants.BoxModels.FreeboxOneR2,
			BoxModels.FreeboxV7R1 => MapperConstants.BoxModels.FreeboxV7R1,
			BoxModels.FreeboxV8R1 => MapperConstants.BoxModels.FreeboxV8R1,
			BoxModels.FreeboxV9R1 => MapperConstants.BoxModels.FreeboxV9R1,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		};
	}
}
