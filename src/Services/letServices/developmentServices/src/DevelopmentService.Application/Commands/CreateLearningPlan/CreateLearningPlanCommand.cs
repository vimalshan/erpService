using MediatR;
using DevelopmentService.Application.DTOs;

namespace DevelopmentService.Application.Commands.CreateLearningPlan;

public record CreateLearningPlanCommand(
    long ReqNum,
    string UserId,
    long PinNum,
    string DevSource,
    string DevNeed,
    long Priority,
    DateTime EntDate) : IRequest<LetPlanDto>;
