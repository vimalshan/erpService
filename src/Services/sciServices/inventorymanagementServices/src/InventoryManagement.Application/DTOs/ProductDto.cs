namespace InventoryManagement.Application.DTOs;

public record ProductDto(
    int ProductId,
    string ProductName,
    string? ProductDescription,
    int? UnitId,
    int? ProductTypeId,
    int? CompanyUnitId,
    int? CreatedBy,
    DateTime? CreatedDate,
    char? MamFlag);
