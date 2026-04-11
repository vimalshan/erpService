using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.EmployeeJournalVouchers.Commands.PostEmployeeJV;

public sealed record PostEmployeeJVCommand(long JvBatchId, string? OracleRefNo, long PostedBy) : IRequest;

public sealed class PostEmployeeJVCommandValidator : AbstractValidator<PostEmployeeJVCommand>
{
    public PostEmployeeJVCommandValidator()
    {
        RuleFor(x => x.JvBatchId).GreaterThan(0);
        RuleFor(x => x.PostedBy).GreaterThan(0);
    }
}

public sealed class PostEmployeeJVCommandHandler(
    IEmployeeJVRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<PostEmployeeJVCommand>
{
    public async Task Handle(PostEmployeeJVCommand request, CancellationToken cancellationToken)
    {
        var jv = await repository.GetByIdAsync(request.JvBatchId, cancellationToken)
            ?? throw new JournalVoucherNotFoundException(request.JvBatchId);

        jv.Post(request.OracleRefNo, request.PostedBy);

        repository.Update(jv);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
