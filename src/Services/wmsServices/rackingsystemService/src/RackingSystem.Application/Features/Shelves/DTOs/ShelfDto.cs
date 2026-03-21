namespace RackingSystem.Application.Features.Shelves.DTOs;

public record ShelfDto(
    int Id,
    int RackId,
    int ShelfLevel,
    int ShelfPosition,
    string Code,
    decimal? CapacityQty,
    decimal? CapacityWeight,
    bool IsActive,
    DateTime CreatedDate,
    DateTime ModifiedDate
);
