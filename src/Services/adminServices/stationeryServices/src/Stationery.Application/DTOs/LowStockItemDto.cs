namespace Stationery.Application.DTOs;

public record LowStockItemDto(
    long Id,
    string Description,
    long Stock,
    long ReorderLevel
);
