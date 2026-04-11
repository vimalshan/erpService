using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.EmployeeJournalVouchers.Commands.ReverseEmployeeJV;

public sealed record ReverseEmployeeJVCommand(long JvBatchId, long ReversedBy) : IRequest;

public sealed class ReverseEmployeeJVCommandHandler(
    IEmployeeJVRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReverseEmployeeJVCommand>
{
    public async Task Handle(ReverseEmployeeJVCommand request, CancellationToken cancellationToken)
    {
        var jv = await repository.GetByIdAsync(request.JvBatchId, cancellationToken)
            ?? throw new JournalVoucherNotFoundException(request.JvBatchId);

        jv.Reverse(request.ReversedBy);

        repository.Update(jv);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
