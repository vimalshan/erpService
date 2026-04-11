using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.SupplierJournalVouchers.Commands.PostSupplierJV;

public sealed record PostSupplierJVCommand(long JvId, string? OracleRefNo, long PostedBy) : IRequest;

public sealed class PostSupplierJVCommandHandler(
    ISupplierJVRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<PostSupplierJVCommand>
{
    public async Task Handle(PostSupplierJVCommand request, CancellationToken cancellationToken)
    {
        var jv = await repository.GetByIdAsync(request.JvId, cancellationToken)
            ?? throw new JournalVoucherNotFoundException(request.JvId);

        jv.Post(request.OracleRefNo, request.PostedBy);

        repository.Update(jv);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
