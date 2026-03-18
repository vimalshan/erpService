using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Commands.Audits;

public record CreateAuditCommand(
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
) : IRequest<AuditDto>;
