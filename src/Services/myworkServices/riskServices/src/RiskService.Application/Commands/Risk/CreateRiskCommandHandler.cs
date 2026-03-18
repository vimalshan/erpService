using MediatR;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Entities;
using RiskService.Domain.Events;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Commands.Risk;

public class CreateRiskCommandHandler(IRiskRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRiskCommand, long>
{
    public async Task<long> Handle(CreateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = new RiskAggregate
        {
            ApplicableTo = request.ApplicableTo,
            OrganizationId = request.OrganizationId,
            BusinessId = request.BusinessId,
            DivisionId = request.DivisionId,
            UnitId = request.UnitId,
            FunctionId = request.FunctionId,
            EventTitle = request.EventTitle,
            Description = request.Description,
            TypeId = request.TypeId,
            ImpactId = request.ImpactId,
            ProbabilityId = request.ProbabilityId,
            RatingId = request.RatingId,
            ResidualImpactId = request.ResidualImpactId,
            ResidualProbabilityId = request.ResidualProbabilityId,
            ResidualRatingId = request.ResidualRatingId,
            ResponseId = request.ResponseId,
            MitigationFlag = 'N',
            OwnerId = request.OwnerId,
            ApprovalStatus = 'E',
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        foreach (var cause in request.Causes)
        {
            risk.AddCause(new RiskCause
            {
                RiskId = risk.Id,
                Description = cause.Description,
                LastModifiedBy = request.CreatedBy,
                LastModifiedOn = DateTime.UtcNow
            });
        }

        foreach (var control in request.Controls)
        {
            risk.AddControl(new RiskControl
            {
                RiskId = risk.Id,
                Description = control.Description,
                FileName = control.FileName,
                LastModifiedBy = request.CreatedBy,
                LastModifiedOn = DateTime.UtcNow
            });
        }

        risk.AddDomainEvent(new RiskCreatedEvent(risk.Id, risk.EventTitle));

        await repository.AddAsync(risk, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return risk.Id;
    }
}
