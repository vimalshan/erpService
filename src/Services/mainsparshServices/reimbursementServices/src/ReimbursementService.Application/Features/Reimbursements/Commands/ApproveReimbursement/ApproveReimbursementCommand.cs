using FluentValidation;
using MediatR;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.ApproveReimbursement;

public sealed record ApproveReimbursementCommand(long ReimId, long ApprovedBy, int ApprovalLevel) : IRequest;

public sealed class ApproveReimbursementCommandValidator : AbstractValidator<ApproveReimbursementCommand>
{
    public ApproveReimbursementCommandValidator()
    {
        RuleFor(x => x.ReimId).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
        RuleFor(x => x.ApprovalLevel).GreaterThan(0);
    }
}

public sealed class ApproveReimbursementCommandHandler(IReimbursementRepository repository)
    : IRequestHandler<ApproveReimbursementCommand>
{
    public async Task Handle(ApproveReimbursementCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken)
            ?? throw new KeyNotFoundException($"Reimbursement {request.ReimId} not found.");
        entity.Approve(request.ApprovedBy, request.ApprovalLevel);
        await repository.UpdateAsync(entity, cancellationToken);
    }
}
