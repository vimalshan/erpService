using MediatR;
using CashManagement.Application.Commands.BankTransaction;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.BankTransaction;

public class RecordBankTransactionHandler : IRequestHandler<RecordBankTransactionCommand, BankTransactionDto>
{
    private readonly IBankTransactionRepository _repository;
    private readonly IBankAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordBankTransactionHandler(IBankTransactionRepository repository,
        IBankAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BankTransactionDto> Handle(RecordBankTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.BankAccountId, cancellationToken)
            ?? throw new DomainException($"Bank account {request.BankAccountId} not found.");

        var txn = Domain.Entities.BankTransaction.Create(
            request.BankAccountId, request.TxnType, request.Amount,
            request.Reference, request.Remarks, request.CreatedBy);

        await _repository.AddAsync(txn, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BankTransactionDto(txn.BankTxnId, txn.BankAccountId, txn.TxnType.ToString(),
            txn.Amount, txn.TxnDate, txn.Reference, txn.Remarks, txn.Status.ToString(),
            txn.CreatedBy, txn.CreatedOn);
    }
}
