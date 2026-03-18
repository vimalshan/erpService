using MediatR;
using RiskService.Application.DTOs;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Queries.Mitigation;

public record GetMitigationsByRiskIdQuery(long RiskId) : IRequest<IReadOnlyList<MitigationDto>>;

public class GetMitigationsByRiskIdQueryHandler(IMitigationRepository repository)
    : IRequestHandler<GetMitigationsByRiskIdQuery, IReadOnlyList<MitigationDto>>
{
    public async Task<IReadOnlyList<MitigationDto>> Handle(GetMitigationsByRiskIdQuery request, CancellationToken cancellationToken)
    {
        var mitigations = await repository.GetByRiskIdAsync(request.RiskId, cancellationToken);
        return mitigations.Select(m => new MitigationDto
        {
            Id = m.Id,
            RiskId = m.RiskId,
            Action = m.Action,
            OriginalDueDate = m.OriginalDueDate,
            DueDate = m.DueDate,
            OwnerId = m.OwnerId,
            ReviewerId = m.ReviewerId,
            Status = m.Status,
            ProbabilityReduction = m.ProbabilityReduction,
            ImpactReduction = m.ImpactReduction,
            Attachment = m.Attachment,
            Actions = m.Actions.Select(a => new MitigationActionDto(
                a.Id, a.MitigationId, a.DueDate, a.Status, a.ApprovalStatus, a.Comments, a.CompletionDate
            )).ToList()
        }).ToList();
    }
}
