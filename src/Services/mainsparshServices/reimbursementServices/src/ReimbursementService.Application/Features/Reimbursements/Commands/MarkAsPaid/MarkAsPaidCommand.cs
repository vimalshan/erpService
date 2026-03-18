using FluentValidation;
using MediatR;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.MarkAsPaid;

public sealed record MarkAsPaidCommand(long ReimId, DateOnly PaymentDate, long UpdatedBy) : IRequest;

public sealed class MarkAsPaidCommandValidator : AbstractValidator<MarkAsPaidCommand>
{
    public MarkAsPaidCommandValidator()
    {
        RuleFor(x => x.ReimId).GreaterThan(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public sealed class MarkAsPaidCommandHandler(IReimbursementRepository repository)
    : IRequestHandler<MarkAsPaidCommand>
{
    public async Task Handle(MarkAsPaidCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken)
            ?? throw new KeyNotFoundException($"Reimbursement {request.ReimId} not found.");
        entity.MarkAsPaid(request.PaymentDate, request.UpdatedBy);
        await repository.UpdateAsync(entity, cancellationToken);
    }
}
