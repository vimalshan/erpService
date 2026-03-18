using MediatR;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.Application.Features.Reimbursements.Commands.SubmitReimbursement;

public sealed record SubmitReimbursementCommand(long ReimId) : IRequest;

public sealed class SubmitReimbursementCommandHandler(IReimbursementRepository repository)
    : IRequestHandler<SubmitReimbursementCommand>
{
    public async Task Handle(SubmitReimbursementCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.ReimId, cancellationToken)
            ?? throw new KeyNotFoundException($"Reimbursement {request.ReimId} not found.");
        entity.Submit();
        await repository.UpdateAsync(entity, cancellationToken);
    }
}
