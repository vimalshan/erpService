namespace UnitService.Application.DTOs;

public record EquipmentDto(
    int EquipmentId,
    string EquipmentName,
    string UnitCode,
    string Category,
    DateTime StartDate,
    DateTime? CloseDate,
    int LastModifiedBy,
    DateTime LastModifiedOn);
