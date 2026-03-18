using MediatR;
using RiskService.Domain.Interfaces;

namespace RiskService.Application.Commands.Risk;

public record SubmitRiskCommand(long RiskId, long SubmittedBy) : IRequest<bool>;

public class SubmitRiskCommandHandler(IRiskRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitRiskCommand, bool>
{
    public async Task<bool> Handle(SubmitRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await repository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk is null) return false;

        risk.Submit(request.SubmittedBy);
        repository.Update(risk);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public record ApproveRiskCommand(long RiskId, long ApprovedBy, string Remarks) : IRequest<bool>;

public class ApproveRiskCommandHandler(IRiskRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveRiskCommand, bool>
{
    public async Task<bool> Handle(ApproveRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await repository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk is null) return false;

        risk.Approve(request.ApprovedBy, request.Remarks);
        repository.Update(risk);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public record CancelRiskCommand(long RiskId, long CancelledBy, string Reason) : IRequest<bool>;

public class CancelRiskCommandHandler(IRiskRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CancelRiskCommand, bool>
{
    public async Task<bool> Handle(CancelRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await repository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk is null) return false;

        risk.Cancel(request.CancelledBy, request.Reason);
        repository.Update(risk);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
