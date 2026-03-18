namespace AuditService.Application.DTOs;

public record AuditDto(
    long AuditId,
    string AuditName,
    long AuditUnit,
    DateTime AuditFrom,
    DateTime AuditTo,
    string AuditDefLocation,
    char AuditStatus,
    decimal AuditCreatedBy,
    DateTime AuditCreatedOn,
    DateTime AuditPlanFrom,
    DateTime AuditPlanTo,
    char? AuditCompleted,
    string? AuditFirmName,
    long? AuditProcess,
    int ObservationCount
);

public record CreateAuditRequest(
    long AuditId,
    string AuditName,
    long AuditUnit,
    DateTime AuditFrom,
    DateTime AuditTo,
    string AuditDefLocation,
    DateTime AuditPlanFrom,
    DateTime AuditPlanTo,
    decimal CreatedBy,
    long? AuditProcess = null,
    string? AuditFirmName = null
);

public record UpdateAuditRequest(
    string AuditName,
    string AuditDefLocation,
    DateTime AuditFrom,
    DateTime AuditTo,
    decimal UpdatedBy
);
