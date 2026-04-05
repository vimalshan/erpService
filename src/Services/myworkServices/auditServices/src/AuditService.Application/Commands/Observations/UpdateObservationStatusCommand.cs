using MediatR;

namespace AuditService.Application.Commands.Observations;

public record UpdateObservationStatusCommand(
    long ObvId,
    string NewStatus,
    long ModifiedBy
) : IRequest<bool>;
