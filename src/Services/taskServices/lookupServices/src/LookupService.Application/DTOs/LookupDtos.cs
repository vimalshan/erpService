namespace LookupService.Application.DTOs;

public record LovTypeMasterDto(string LovTypeCode, string? LovTypeName);

public record LovMasterDto(long LovId, string? LovType, string? LovName);

public record LovUnitMapDto(decimal LuMapId, long? LuLovId, string? LuUnitCode, string? LuFlag);

public record LovPanelMapDto(long? LpLovId, decimal? LpPanelId, string? LpFlag);

public record PanelMasterDto(decimal PanelId, string? PanelName);

public record ProcessMasterDto(decimal ProcessId, string? ProcessName, string? ProcessLivFlag);

public record UnitProcessMapDto(decimal UpMapId, string? UpUnitCode, decimal? UpProcessId);

public record UnitLovAccessMasterDto(
    decimal UaAccessMastId,
    decimal? UaUnitLovMapId,
    decimal? UaDepartmentId,
    decimal? UaProcessId);

public record UnitLovAccessDetailDto(
    decimal UdAccessDetId,
    decimal? UdAccessMastId,
    string? UdAccessType,
    string? UdEmpSysId,
    decimal? UdEscDays,
    string? UdEffDat,
    string? UdClsDat,
    decimal? UdUpdatedBy,
    DateTime? UdUpdatedOn);
