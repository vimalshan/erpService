namespace EnergyService.Application.DTOs;

public record EcProcessDto(
    int EcProcessId,
    string EcProcessDesc,
    string EcUnitCode,
    string EcCloseFlag,
    int LastModifiedBy,
    DateTime LastModifiedOn);
