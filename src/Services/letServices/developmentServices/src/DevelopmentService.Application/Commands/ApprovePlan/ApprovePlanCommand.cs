using MediatR;

namespace DevelopmentService.Application.Commands.ApprovePlan;

public record ApprovePlanCommand(
    long ReqNum,
    char AppStatus,
    char? BhrStatus = null) : IRequest<bool>;
