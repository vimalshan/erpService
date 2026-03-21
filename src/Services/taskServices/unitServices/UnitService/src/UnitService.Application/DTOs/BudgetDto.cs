namespace UnitService.Application.DTOs;

public record BudgetDto(
    string UnitCode,
    decimal EquipmentId,
    DateTime StartDate,
    DateTime? CloseDate);
