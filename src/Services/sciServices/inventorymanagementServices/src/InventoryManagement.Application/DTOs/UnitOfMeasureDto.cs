namespace InventoryManagement.Application.DTOs;

public record UnitOfMeasureDto(
    int UnitId,
    string UnitCode,
    string UnitOfMeasurement,
    int UnitClassId,
    char BaseUnitFlag,
    string? Description);
