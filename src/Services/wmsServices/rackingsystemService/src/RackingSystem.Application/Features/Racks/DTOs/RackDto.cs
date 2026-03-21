namespace RackingSystem.Application.Features.Racks.DTOs;

public record RackDto(
    int Id,
    int ZoneId,
    string Code,
    string? RackType,
    decimal? MaxLoadWeight,
    bool IsActive,
    DateTime CreatedDate,
    DateTime ModifiedDate,
    IEnumerable<ShelfSummaryDto> Shelves
);

public record ShelfSummaryDto(int Id, int ShelfLevel, int ShelfPosition, string Code, bool IsActive);
