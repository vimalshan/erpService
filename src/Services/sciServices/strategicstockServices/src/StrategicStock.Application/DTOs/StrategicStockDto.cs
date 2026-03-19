namespace StrategicStock.Application.DTOs;

public sealed record StrategicStockDto
{
    public int StrategicStockId { get; init; }
    public int? CompanyUnitId { get; init; }
    public int SciItemId { get; init; }
    public string? StrategicStockType { get; init; }
    public long? MaxQty { get; init; }
    public string? EffectiveDate { get; init; }
    public string? ClosureDate { get; init; }
    public int? SciUserIdCreated { get; init; }
    public DateTime CreationDate { get; init; }
    public int? SciUserIdModified { get; init; }
    public string? ModifiedDate { get; init; }
    public long? FilledQty { get; init; }
}
