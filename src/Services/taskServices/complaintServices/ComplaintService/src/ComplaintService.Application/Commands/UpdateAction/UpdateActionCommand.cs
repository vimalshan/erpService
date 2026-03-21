using MediatR;

namespace ComplaintService.Application.Commands.UpdateAction;

public record UpdateActionCommand(
    decimal ActionNum,
    char ActionLevel,
    string Solution,
    decimal ActionBy
) : IRequest<Unit>;
