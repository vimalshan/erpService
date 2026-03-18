using MediatR;

namespace AuditService.Application.Commands.Observations;

public record UpdateObservationStatusCommand(
    long ObvId,
    char NewStatus,
    long ModifiedBy
) : IRequest<bool>;
