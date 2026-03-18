using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Commands.Observations;

public record CreateObservationCommand(
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
) : IRequest<ObservationDto>;
