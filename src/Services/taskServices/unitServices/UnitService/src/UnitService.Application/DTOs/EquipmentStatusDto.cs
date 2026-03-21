namespace UnitService.Application.DTOs;

public record EquipmentStatusDto(
    int StatusId,
    int EquipmentId,
    string StatusDescription,
    string StatusCode,
    string StartDate,
    string? CloseDate,
    string? Remarks,
    long? Hours,
    string? FilePath,
    int? CreatedBy,
    string? CreatedOn);
