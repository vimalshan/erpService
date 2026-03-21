using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.DTOs;

public sealed record ShipmentLineDto(
    int ShipmentLineId,
    int ShipmentId,
    int? SoLineId,
    int ProductId,
    int BinId,
    decimal QuantityShipped,
    decimal? UnitPrice,
    string? LotNumber,
    DateOnly? ExpiryDate,
    string? Notes)
{
    public static ShipmentLineDto FromEntity(ShipmentLine l) => new(
        l.Id, l.ShipmentId, l.SoLineId, l.ProductId, l.BinId,
        l.QuantityShipped, l.UnitPrice, l.LotNumber, l.ExpiryDate, l.Notes);
}
