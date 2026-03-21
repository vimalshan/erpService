namespace UnitService.Application.DTOs;

public record AccessDto(
    int AccessId,
    string UnitCode,
    int EmployeeSysId,
    string AccessType,
    DateTime StartDate,
    string? CloseDate,
    string Module,
    int LastModifiedBy,
    DateTime LastModifiedOn);
