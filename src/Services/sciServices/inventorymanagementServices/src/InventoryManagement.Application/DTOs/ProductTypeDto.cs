namespace InventoryManagement.Application.DTOs;

public record ProductTypeDto(
    int ProductTypeId,
    string TypeName,
    string? TypeDescription);
