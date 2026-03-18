namespace Stationery.Application.DTOs;

public record ItemDto(
    long Id,
    string Description,
    long CatId,
    long LocId,
    long UomId,
    string Make,
    long? PricePerUnit,
    long? ReorderLevel,
    long OpeningStock,
    char Closed
);

public record ItemSummaryDto(
    long Id,
    string Description,
    long OpeningStock,
    long? ReorderLevel
);
