namespace AuditService.Application.DTOs;

public record ObservationDto(
    long ObvId,
    long ObvAuditId,
    string ObvTitle,
    string ObvDescription,
    char ObvRisk,
    long ObvAuditee,
    long ObvEsc1,
    long ObvEsc2,
    string ObvManComments,
    string? ObvImplication,
    char ObvStatus,
    DateTime ObvOrgDueDate,
    DateTime? ObvOrgRev1Date,
    DateTime? ObvOrgRev2Date,
    long ObvCreatedBy,
    DateTime ObvCreatedOn,
    string ObvLocation,
    string ObvAuditorName,
    string ObvRemarks,
    char? ObvAppStatus
);

public record CreateObservationRequest(
    long ObvId,
    long AuditId,
    string Title,
    string Description,
    char Risk,
    long Auditee,
    long Esc1,
    long Esc2,
    string ManComments,
    DateTime OrgDueDate,
    string Location,
    string AuditorName,
    string Remarks,
    long CreatedBy
);

public record UpdateObservationStatusRequest(
    char NewStatus,
    long ModifiedBy
);
