namespace VehicleTracking.Domain.ValueObjects;

public record WeightInfo
{
    public decimal? TyreWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public decimal? NetWeight { get; init; }

    public static WeightInfo Create(decimal? tyreWeight, decimal? grossWeight, decimal? netWeight)
    {
        return new WeightInfo
        {
            TyreWeight = tyreWeight,
            GrossWeight = grossWeight,
            NetWeight = netWeight
        };
    }
}
