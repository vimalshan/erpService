using DevelopmentService.Application.Commands.CreateLearningPlan;
using DevelopmentService.Application.Commands.ApprovePlan;
using DevelopmentService.Application.Commands.CreateBhrPlan;
using DevelopmentService.Application.DTOs;
using MediatR;

namespace DevelopmentService.API.GraphQL.Mutations;

public class DevelopmentMutation
{
    public async Task<LetPlanDto> CreateLearningPlan(
        CreateLearningPlanCommand input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> ApprovePlan(
        ApprovePlanCommand input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<LetBhrPlanDto> CreateBhrPlan(
        CreateBhrPlanCommand input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);
}
