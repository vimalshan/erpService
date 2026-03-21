namespace EnergyService.Application.DTOs;

public record EcProcessAccessDto(
    int? PaId,
    int PaProcessId,
    int PaEmpSysId,
    DateTime PaStartDate,
    DateTime? PaCloseDate,
    int PaLastModifiedBy,
    string PaLastModifiedOn);
