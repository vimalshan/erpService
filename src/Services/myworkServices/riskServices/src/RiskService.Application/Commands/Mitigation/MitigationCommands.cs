using MediatR;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Entities;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Commands.Mitigation;

public record CreateMitigationCommand : IRequest<long>
{
    public long RiskId { get; init; }
    public string Action { get; init; } = default!;
    public DateTime OriginalDueDate { get; init; }
    public DateTime DueDate { get; init; }
    public long OwnerId { get; init; }
    public long ReviewerId { get; init; }
    public long CreatedBy { get; init; }
}

public class CreateMitigationCommandHandler(
    IRiskRepository riskRepository,
    IMitigationRepository mitigationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateMitigationCommand, long>
{
    public async Task<long> Handle(CreateMitigationCommand request, CancellationToken cancellationToken)
    {
        var mitigation = new RiskMitigation
        {
            RiskId = request.RiskId,
            Action = request.Action,
            OriginalDueDate = request.OriginalDueDate,
            DueDate = request.DueDate,
            OwnerId = request.OwnerId,
            ReviewerId = request.ReviewerId,
            Status = 'L',
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        await mitigationRepository.AddAsync(mitigation, cancellationToken);

        var risk = await riskRepository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk is not null)
        {
            risk.AddMitigation(mitigation);
            riskRepository.Update(risk);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mitigation.Id;
    }
}

public record AddMitigationActionCommand(long MitigationId, DateTime DueDate, string Comments, long CreatedBy) : IRequest<long>;

public class AddMitigationActionCommandHandler(
    IMitigationRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddMitigationActionCommand, long>
{
    public async Task<long> Handle(AddMitigationActionCommand request, CancellationToken cancellationToken)
    {
        var mitigation = await repository.GetByIdAsync(request.MitigationId, cancellationToken);
        if (mitigation is null) return 0;

        var action = new RiskMitigationAction
        {
            MitigationId = request.MitigationId,
            DueDate = request.DueDate,
            Status = 'N',
            ApprovalStatus = 'E',
            Comments = request.Comments,
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        mitigation.AddAction(action);
        repository.Update(mitigation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return action.Id;
    }
}
