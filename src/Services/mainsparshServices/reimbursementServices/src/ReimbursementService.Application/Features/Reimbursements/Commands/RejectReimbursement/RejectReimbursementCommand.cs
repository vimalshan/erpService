using FluentValidation;
using MediatR;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.RejectReimbursement;

public sealed record RejectReimbursementCommand(long ReimId, long RejectedBy, string Reason) : IRequest;

public sealed class RejectReimbursementCommandValidator : AbstractValidator<RejectReimbursementCommand>
{
    public RejectReimbursementCommandValidator()
    {
        RuleFor(x => x.ReimId).GreaterThan(0);
        RuleFor(x => x.RejectedBy).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(2000);
    }
}

public sealed class RejectReimbursementCommandHandler(IReimbursementRepository repository)
    : IRequestHandler<RejectReimbursementCommand>
{
    public async Task Handle(RejectReimbursementCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken)
            ?? throw new KeyNotFoundException($"Reimbursement {request.ReimId} not found.");
        entity.Reject(request.RejectedBy, request.Reason);
        await repository.UpdateAsync(entity, cancellationToken);
    }
}
