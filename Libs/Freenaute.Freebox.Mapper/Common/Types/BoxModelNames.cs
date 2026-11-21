namespace Freenaute.Freebox.Mapper.Common.Types;

/// <summary>Classifies known discovery model names while preserving the server's original string.</summary>
public static class BoxModelNames
{
    public static BoxModels? Classify(string? value)
    {
        return value switch
        {
            "fbxgw-r1/full" => BoxModels.FreeboxServerV6R1,
            "fbxgw-r2/full" => BoxModels.FreeboxServerV6R2,
            "fbxgw-r1/mini" => BoxModels.FreeboxMiniR1,
            "fbxgw-r2/mini" => BoxModels.FreeboxMiniR2,
            "fbxgw-r1/one" => BoxModels.FreeboxOneR1,
            "fbxgw-r2/one" => BoxModels.FreeboxOneR2,
            "fbxgw7-r1/full" => BoxModels.FreeboxV7R1,
            "fbxgw8-r1/full" => BoxModels.FreeboxV8R1,
            "fbxgw9-r1/full" or "fbxgw9-r1" => BoxModels.FreeboxV9R1,
            _ => null
        };
    }

    public static string GetWireName(this BoxModels value)
    {
        return value switch
        {
            BoxModels.FreeboxServerV6R1 => "fbxgw-r1/full",
            BoxModels.FreeboxServerV6R2 => "fbxgw-r2/full",
            BoxModels.FreeboxMiniR1 => "fbxgw-r1/mini",
            BoxModels.FreeboxMiniR2 => "fbxgw-r2/mini",
            BoxModels.FreeboxOneR1 => "fbxgw-r1/one",
            BoxModels.FreeboxOneR2 => "fbxgw-r2/one",
            BoxModels.FreeboxV7R1 => "fbxgw7-r1/full",
            BoxModels.FreeboxV8R1 => "fbxgw8-r1/full",
            BoxModels.FreeboxV9R1 => "fbxgw9-r1/full",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}
