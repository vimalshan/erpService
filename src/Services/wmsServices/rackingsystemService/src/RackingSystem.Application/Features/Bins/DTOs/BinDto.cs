namespace RackingSystem.Application.Features.Bins.DTOs;

public record BinDto(
    int Id,
    int ZoneId,
    int? ShelfId,
    string Code,
    string? Barcode,
    string? BinType,
    decimal? CapacityQty,
    decimal? CapacityWeight,
    decimal? CapacityVolume,
    string Status,
    bool IsActive,
    decimal? UtilizationPct,
    DateTime CreatedDate,
    DateTime ModifiedDate
);
