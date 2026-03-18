using MediatR;
using DevelopmentService.Application.DTOs;

namespace DevelopmentService.Application.Commands.CreateBhrPlan;

public record CreateBhrPlanCommand(
    long ReqNum,
    string UserId,
    string TrainingProgram,
    decimal TrainingCode,
    decimal Priority,
    char BhrAccept) : IRequest<LetBhrPlanDto>;
