using MediatR;
using CashManagement.Application.Commands.CashTransaction;
using CashManagement.Application.DTOs;
using CashManagement.Domain.Entities;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.Interfaces;
using CashManagement.Domain.Interfaces.Repositories;

namespace CashManagement.Application.Handlers.CashTransaction;

public class RecordCashReceiptHandler : IRequestHandler<RecordCashReceiptCommand, CashTransactionDto>
{
    private readonly ICashTransactionRepository _repository;
    private readonly ICashUnitRepository _cashUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordCashReceiptHandler(ICashTransactionRepository repository,
        ICashUnitRepository cashUnitRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cashUnitRepository = cashUnitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CashTransactionDto> Handle(RecordCashReceiptCommand request, CancellationToken cancellationToken)
    {
        var unit = await _cashUnitRepository.GetByIdAsync(request.CashUnitId, cancellationToken)
            ?? throw new DomainException($"Cash unit {request.CashUnitId} not found.");

        var txn = Domain.Entities.CashTransaction.CreateReceipt(
            request.CashUnitId, request.Amount, request.Source,
            request.RefNo, request.Remarks, request.CreatedBy, request.AuthorizedBy);

        await _repository.AddAsync(txn, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(txn);
    }

    private static CashTransactionDto MapToDto(Domain.Entities.CashTransaction t)
        => new(t.CashTxnId, t.CashUnitId, t.TxnType.ToString(), t.Amount,
               t.Source, t.PayeeId, t.RefNo, t.TxnDate, t.Remarks,
               t.Status.ToString(), t.AuthorizedBy, t.CreatedBy, t.CreatedOn);
}

public class RecordCashDisbursementHandler : IRequestHandler<RecordCashDisbursementCommand, CashTransactionDto>
{
    private readonly ICashTransactionRepository _repository;
    private readonly ICashUnitRepository _cashUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordCashDisbursementHandler(ICashTransactionRepository repository,
        ICashUnitRepository cashUnitRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cashUnitRepository = cashUnitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CashTransactionDto> Handle(RecordCashDisbursementCommand request, CancellationToken cancellationToken)
    {
        var availableCash = await _cashUnitRepository.GetCashInHandAsync(
            request.CashUnitId, DateTime.UtcNow, cancellationToken);

        if (request.Amount > availableCash)
            throw new InsufficientCashException(availableCash, request.Amount);

        var txn = Domain.Entities.CashTransaction.CreateDisbursement(
            request.CashUnitId, request.Amount, request.Source,
            request.PayeeId, request.RefNo, request.Remarks,
            request.CreatedBy, request.AuthorizedBy);

        await _repository.AddAsync(txn, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new(txn.CashTxnId, txn.CashUnitId, txn.TxnType.ToString(), txn.Amount,
            txn.Source, txn.PayeeId, txn.RefNo, txn.TxnDate, txn.Remarks,
            txn.Status.ToString(), txn.AuthorizedBy, txn.CreatedBy, txn.CreatedOn);
    }
}
