using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Queries.GetPlans;
using DevelopmentService.Application.Queries.GetCompetencyIndicators;
using MediatR;

namespace DevelopmentService.API.GraphQL.Queries;

public class DevelopmentQuery
{
    public async Task<IEnumerable<LetPlanDto>> GetLearningPlans(
        string? userId, char? status,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPlansQuery(userId, status), ct);

    public async Task<IEnumerable<CompetencyIndDto>> GetCompetencyIndicators(
        long? compNum, string? band,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetCompetencyIndicatorsQuery(compNum, band), ct);
}
