using MediatR;
using RiskService.Application.Commands.Risk;
using RiskService.Application.Commands.Mitigation;
using RiskService.Application.Commands.SelfAssessment;

namespace RiskService.API.GraphQL.Mutations;

public class RiskMutation
{
    public async Task<long> CreateRisk([Service] IMediator mediator, CreateRiskCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateRisk([Service] IMediator mediator, UpdateRiskCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> SubmitRisk([Service] IMediator mediator, long riskId, long submittedBy, CancellationToken ct)
        => await mediator.Send(new SubmitRiskCommand(riskId, submittedBy), ct);

    public async Task<bool> ApproveRisk([Service] IMediator mediator, long riskId, long approvedBy, string remarks, CancellationToken ct)
        => await mediator.Send(new ApproveRiskCommand(riskId, approvedBy, remarks), ct);

    public async Task<bool> CancelRisk([Service] IMediator mediator, long riskId, long cancelledBy, string reason, CancellationToken ct)
        => await mediator.Send(new CancelRiskCommand(riskId, cancelledBy, reason), ct);

    public async Task<long> CreateMitigation([Service] IMediator mediator, CreateMitigationCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<long> CreateSelfAssessment([Service] IMediator mediator, CreateSelfAssessmentCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> CompleteSelfAssessment([Service] IMediator mediator, long assessmentId, long completedBy, CancellationToken ct)
        => await mediator.Send(new CompleteSelfAssessmentCommand(assessmentId, completedBy), ct);
}
