using MediatR;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.Risk;
using RiskService.Application.Queries.RiskType;
using RiskService.Application.Queries.Mitigation;
using RiskService.Application.Queries.SelfAssessment;

namespace RiskService.API.GraphQL.Queries;

public class RiskQuery
{
    public async Task<IReadOnlyList<RiskDto>> GetRisks([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRisksQuery(), ct);

    public async Task<RiskDto?> GetRiskById([Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetRiskByIdQuery(id), ct);

    public async Task<IReadOnlyList<MitigationDto>> GetMitigationsByRiskId([Service] IMediator mediator, long riskId, CancellationToken ct)
        => await mediator.Send(new GetMitigationsByRiskIdQuery(riskId), ct);

    public async Task<IReadOnlyList<SelfAssessmentDto>> GetPendingSelfAssessments([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPendingSelfAssessmentsQuery(), ct);

    public async Task<IReadOnlyList<RiskTypeDto>> GetRiskTypes([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRiskTypesQuery(), ct);

    public async Task<IReadOnlyList<RiskImpactDto>> GetRiskImpacts([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRiskImpactsQuery(), ct);

    public async Task<IReadOnlyList<RiskProbabilityDto>> GetRiskProbabilities([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRiskProbabilitiesQuery(), ct);

    public async Task<IReadOnlyList<RiskRatingDto>> GetRiskRatings([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRiskRatingsQuery(), ct);
}
