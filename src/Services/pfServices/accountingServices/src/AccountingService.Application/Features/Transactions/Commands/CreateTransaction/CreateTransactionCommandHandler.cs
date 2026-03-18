using AccountingService.Application.Common.Interfaces;
using AccountingService.Application.DTOs;
using AccountingService.Domain.Entities;
using MediatR;

namespace AccountingService.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDetailDto>
{
    private readonly IApplicationDbContext _context;

    public CreateTransactionCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<TransactionDetailDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = TransactionDetail.Create(
            request.TrustCode, request.TransactionId, request.TransactionCode,
            request.TransactionDate, request.Amount, request.TypeCode,
            request.ModifiedBy, request.FinYear, request.JvVoucherType, request.JvNo,
            request.TransactionType, request.Remarks, request.MemberNo,
            request.ReferenceType, request.ContributionRefNo, request.TrnSubType);

        _context.TransactionDetails.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new TransactionDetailDto(entity.TdTrustCode, entity.TransactionId,
            entity.TdTransactionCode, entity.TdTransactionType, entity.TdTransactionDate,
            entity.TdAmount, entity.TdRemarks, entity.TdMemberNo, entity.TdTypeCode,
            entity.TdFinyear, entity.TdJvVoucherType, entity.TdJvNo, entity.IsCancelled);
    }
}
