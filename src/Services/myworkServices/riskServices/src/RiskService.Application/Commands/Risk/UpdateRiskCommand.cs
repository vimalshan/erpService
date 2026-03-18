using MediatR;

namespace RiskService.Application.Commands.Risk;

public record UpdateRiskCommand : IRequest<bool>
{
    public long Id { get; init; }
    public string EventTitle { get; init; } = default!;
    public string Description { get; init; } = default!;
    public long TypeId { get; init; }
    public long ImpactId { get; init; }
    public long ProbabilityId { get; init; }
    public long RatingId { get; init; }
    public long ResidualImpactId { get; init; }
    public long ResidualProbabilityId { get; init; }
    public long ResidualRatingId { get; init; }
    public long ResponseId { get; init; }
    public long OwnerId { get; init; }
    public long ModifiedBy { get; init; }
}

public class UpdateRiskCommandHandler(
    Domain.Interfaces.IRiskRepository repository,
    Domain.Interfaces.IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRiskCommand, bool>
{
    public async Task<bool> Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (risk is null) return false;

        risk.EventTitle = request.EventTitle;
        risk.Description = request.Description;
        risk.TypeId = request.TypeId;
        risk.ImpactId = request.ImpactId;
        risk.ProbabilityId = request.ProbabilityId;
        risk.RatingId = request.RatingId;
        risk.ResidualImpactId = request.ResidualImpactId;
        risk.ResidualProbabilityId = request.ResidualProbabilityId;
        risk.ResidualRatingId = request.ResidualRatingId;
        risk.ResponseId = request.ResponseId;
        risk.OwnerId = request.OwnerId;
        risk.ModifiedBy = request.ModifiedBy;
        risk.ModifiedOn = DateTime.UtcNow;

        repository.Update(risk);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
