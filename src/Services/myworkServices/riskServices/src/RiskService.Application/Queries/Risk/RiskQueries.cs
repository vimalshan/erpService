using MediatR;
using RiskService.Application.DTOs;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Queries.Risk;

public record GetRiskByIdQuery(long Id) : IRequest<RiskDto?>;

public class GetRiskByIdQueryHandler(IRiskRepository repository)
    : IRequestHandler<GetRiskByIdQuery, RiskDto?>
{
    public async Task<RiskDto?> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var risk = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (risk is null) return null;

        return new RiskDto
        {
            Id = risk.Id,
            ApplicableTo = risk.ApplicableTo,
            OrganizationId = risk.OrganizationId,
            BusinessId = risk.BusinessId,
            DivisionId = risk.DivisionId,
            UnitId = risk.UnitId,
            FunctionId = risk.FunctionId,
            EventTitle = risk.EventTitle,
            Description = risk.Description,
            TypeId = risk.TypeId,
            TypeName = risk.Type?.Name,
            ImpactId = risk.ImpactId,
            ProbabilityId = risk.ProbabilityId,
            RatingId = risk.RatingId,
            ResidualImpactId = risk.ResidualImpactId,
            ResidualProbabilityId = risk.ResidualProbabilityId,
            ResidualRatingId = risk.ResidualRatingId,
            ResponseId = risk.ResponseId,
            MitigationFlag = risk.MitigationFlag,
            OwnerId = risk.OwnerId,
            ApprovalStatus = risk.ApprovalStatus,
            CancelDate = risk.CancelDate,
            CancelReason = risk.CancelReason,
            CreatedBy = risk.CreatedBy,
            CreatedOn = risk.CreatedOn,
            ModifiedBy = risk.ModifiedBy,
            ModifiedOn = risk.ModifiedOn,
            Causes = risk.Causes.Select(c => new RiskCauseDto(c.Id, c.RiskId, c.Description)).ToList(),
            Controls = risk.Controls.Select(c => new RiskControlDto(c.Id, c.RiskId, c.Description, c.FileName, c.ImpactReductionPercent, c.ProbabilityReductionPercent)).ToList(),
            ImpactMaps = risk.ImpactMaps.Select(i => new RiskImpactMapDto(i.Id, i.RiskId, i.Description)).ToList(),
            Events = risk.Events.Select(e => new RiskEventDto(e.Id, e.RiskId, e.Description, e.EventDate)).ToList(),
            Mitigations = risk.Mitigations.Select(m => new MitigationDto
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
                Actions = m.Actions.Select(a => new MitigationActionDto(a.Id, a.MitigationId, a.DueDate, a.Status, a.ApprovalStatus, a.Comments, a.CompletionDate)).ToList()
            }).ToList()
        };
    }
}

public record GetAllRisksQuery : IRequest<IReadOnlyList<RiskDto>>;

public class GetAllRisksQueryHandler(IRiskRepository repository)
    : IRequestHandler<GetAllRisksQuery, IReadOnlyList<RiskDto>>
{
    public async Task<IReadOnlyList<RiskDto>> Handle(GetAllRisksQuery request, CancellationToken cancellationToken)
    {
        var risks = await repository.GetAllAsync(cancellationToken);
        return risks.Select(risk => new RiskDto
        {
            Id = risk.Id,
            ApplicableTo = risk.ApplicableTo,
            OrganizationId = risk.OrganizationId,
            BusinessId = risk.BusinessId,
            EventTitle = risk.EventTitle,
            Description = risk.Description,
            TypeId = risk.TypeId,
            TypeName = risk.Type?.Name,
            ApprovalStatus = risk.ApprovalStatus,
            MitigationFlag = risk.MitigationFlag,
            OwnerId = risk.OwnerId,
            CreatedBy = risk.CreatedBy,
            CreatedOn = risk.CreatedOn
        }).ToList();
    }
}
