using MediatR;
using TransactionProcessing.Application.Commands;
using TransactionProcessing.Application.DTOs;
using TransactionProcessing.Domain.Entities;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Application.Handlers;

public sealed class ProcessDealSettlementHandler(IUnitOfWork uow, IEventPublisher publisher)
    : IRequestHandler<ProcessDealSettlementCommand, DealSettlementDto>
{
    public async Task<DealSettlementDto> Handle(ProcessDealSettlementCommand cmd, CancellationToken ct)
    {
        var txn = FinancialTransaction.Create(
            "SETTLEMENT", cmd.SettlementType.ToString(), cmd.NetAmount,
            cmd.CurrencyId, cmd.ExchangeRate, null,
            "DealTicketing", cmd.DealId, cmd.Remarks, cmd.CreatedBy);

        if (cmd.BatchId.HasValue) txn.AssignToBatch(cmd.BatchId.Value);

        await uow.Transactions.AddAsync(txn, ct);
        await uow.SaveChangesAsync(ct);

        var settlement = DealSettlement.Create(
            txn.TxnId, cmd.DealId, cmd.SetId, cmd.SettlementType[0],
            cmd.SpotRate, cmd.ExchangeRate, cmd.SettlementAmount,
            cmd.GainLossAmount, cmd.PremiumAmount, cmd.WindingFee,
            cmd.NetAmount, cmd.BankAccountId, cmd.CreatedBy);

        await uow.DealSettlements.AddAsync(settlement, ct);

        txn.MarkProcessing(cmd.CreatedBy);
        txn.MarkCompleted(cmd.CreatedBy);
        settlement.MarkProcessed();

        await uow.SaveChangesAsync(ct);

        foreach (var evt in txn.DomainEvents.Concat(settlement.DomainEvents))
            await publisher.PublishAsync(evt, "transaction.settlement.processed", ct);

        txn.ClearDomainEvents();
        settlement.ClearDomainEvents();

        return new DealSettlementDto(
            settlement.SettlementId, settlement.TxnId, settlement.DealId, settlement.SetId,
            settlement.SettlementType.ToString(), settlement.SpotRate, settlement.ExchangeRate,
            settlement.SettlementAmount, settlement.GainLossAmount, settlement.PremiumAmount,
            settlement.WindingFee, settlement.NetAmount, settlement.BankAccountId,
            settlement.ProcessingStatus, settlement.CreatedOn);
    }
}

public sealed class ProcessLoanDisbursementHandler(IUnitOfWork uow, IEventPublisher publisher)
    : IRequestHandler<ProcessLoanDisbursementCommand, LoanDisbursementDto>
{
    public async Task<LoanDisbursementDto> Handle(ProcessLoanDisbursementCommand cmd, CancellationToken ct)
    {
        var txn = FinancialTransaction.Create(
            "DISBURSEMENT", null, cmd.DisbAmount,
            cmd.CurrencyId, cmd.ExchangeRate, null,
            "LoanManagement", cmd.LoanId, cmd.Remarks, cmd.CreatedBy);

        if (cmd.BatchId.HasValue) txn.AssignToBatch(cmd.BatchId.Value);

        await uow.Transactions.AddAsync(txn, ct);
        await uow.SaveChangesAsync(ct);

        var disb = LoanDisbursement.Create(
            txn.TxnId, cmd.LoanId, cmd.DisbId, cmd.DisbAmount,
            cmd.ExchangeRate, cmd.BankAccountId, cmd.CreatedBy);

        await uow.LoanDisbursements.AddAsync(disb, ct);

        txn.MarkProcessing(cmd.CreatedBy);
        txn.MarkCompleted(cmd.CreatedBy);
        disb.MarkProcessed();

        await uow.SaveChangesAsync(ct);

        foreach (var evt in txn.DomainEvents.Concat(disb.DomainEvents))
            await publisher.PublishAsync(evt, "transaction.disbursement.processed", ct);

        txn.ClearDomainEvents();
        disb.ClearDomainEvents();

        return new LoanDisbursementDto(
            disb.DisbProcId, disb.TxnId, disb.LoanId, disb.DisbId,
            disb.DisbAmount, disb.ExchangeRate, disb.ConvertedAmount,
            disb.BankAccountId, disb.ProcessingStatus, disb.CreatedOn);
    }
}

public sealed class ProcessLoanRepaymentHandler(IUnitOfWork uow, IEventPublisher publisher)
    : IRequestHandler<ProcessLoanRepaymentCommand, LoanRepaymentDto>
{
    public async Task<LoanRepaymentDto> Handle(ProcessLoanRepaymentCommand cmd, CancellationToken ct)
    {
        var txn = FinancialTransaction.Create(
            "REPAYMENT", null, cmd.RepayAmount,
            cmd.CurrencyId, cmd.ExchangeRate, null,
            "LoanManagement", cmd.LoanId, cmd.Remarks, cmd.CreatedBy);

        if (cmd.BatchId.HasValue) txn.AssignToBatch(cmd.BatchId.Value);

        await uow.Transactions.AddAsync(txn, ct);
        await uow.SaveChangesAsync(ct);

        var repay = LoanRepayment.Create(
            txn.TxnId, cmd.LoanId, cmd.RepayId, cmd.RepayAmount,
            cmd.ExchangeRate, cmd.BankAccountId, cmd.CreatedBy);

        await uow.LoanRepayments.AddAsync(repay, ct);

        txn.MarkProcessing(cmd.CreatedBy);
        txn.MarkCompleted(cmd.CreatedBy);
        repay.MarkProcessed();

        await uow.SaveChangesAsync(ct);

        foreach (var evt in txn.DomainEvents.Concat(repay.DomainEvents))
            await publisher.PublishAsync(evt, "transaction.repayment.processed", ct);

        txn.ClearDomainEvents();
        repay.ClearDomainEvents();

        return new LoanRepaymentDto(
            repay.RepayProcId, repay.TxnId, repay.LoanId, repay.RepayId,
            repay.RepayAmount, repay.ExchangeRate, repay.ConvertedAmount,
            repay.BankAccountId, repay.ProcessingStatus, repay.CreatedOn);
    }
}

public sealed class ProcessCashTransferHandler(IUnitOfWork uow, IEventPublisher publisher)
    : IRequestHandler<ProcessCashTransferCommand, FinancialTransactionDto>
{
    public async Task<FinancialTransactionDto> Handle(ProcessCashTransferCommand cmd, CancellationToken ct)
    {
        var txn = FinancialTransaction.Create(
            "CASH_TRANSFER", cmd.SubType, cmd.Amount,
            cmd.CurrencyId, cmd.ExchangeRate, null,
            cmd.SourceService, cmd.SourceId, cmd.Remarks, cmd.CreatedBy);

        if (cmd.BatchId.HasValue) txn.AssignToBatch(cmd.BatchId.Value);

        await uow.Transactions.AddAsync(txn, ct);
        await uow.SaveChangesAsync(ct);

        txn.MarkProcessing(cmd.CreatedBy);
        txn.MarkCompleted(cmd.CreatedBy);

        await uow.SaveChangesAsync(ct);

        foreach (var evt in txn.DomainEvents)
            await publisher.PublishAsync(evt, "transaction.cash.transferred", ct);

        txn.ClearDomainEvents();

        return new FinancialTransactionDto(
            txn.TxnId, txn.TxnBatchId, txn.TxnType, txn.TxnSubType,
            txn.TxnAmount, txn.TxnCurrencyId, txn.TxnExchangeRate, txn.TxnBaseAmount,
            txn.TxnReference, txn.TxnSourceService, txn.TxnSourceId,
            txn.TxnStatus, txn.TxnRemarks, txn.CreatedBy, txn.CreatedOn);
    }
}

public sealed class CreateTransactionBatchHandler(IUnitOfWork uow)
    : IRequestHandler<CreateTransactionBatchCommand, TransactionBatchDto>
{
    public async Task<TransactionBatchDto> Handle(CreateTransactionBatchCommand cmd, CancellationToken ct)
    {
        var batch = TransactionBatch.Create(cmd.BatchType, cmd.BatchDate, cmd.CreatedBy);
        await uow.Batches.AddAsync(batch, ct);
        await uow.SaveChangesAsync(ct);

        return new TransactionBatchDto(
            batch.BatchId, batch.BatchType, batch.BatchDate, batch.BatchStatus,
            batch.BatchTotalCount, batch.BatchSuccessCount, batch.BatchFailureCount,
            batch.BatchTotalAmount, batch.CreatedBy, batch.CreatedOn, batch.CompletedOn);
    }
}

public sealed class CompleteTransactionBatchHandler(IUnitOfWork uow, IEventPublisher publisher)
    : IRequestHandler<CompleteTransactionBatchCommand, TransactionBatchDto>
{
    public async Task<TransactionBatchDto> Handle(CompleteTransactionBatchCommand cmd, CancellationToken ct)
    {
        var batch = await uow.Batches.GetByIdAsync(cmd.BatchId, ct)
            ?? throw new KeyNotFoundException($"Batch {cmd.BatchId} not found");

        var txns = await uow.Transactions.GetByBatchIdAsync(cmd.BatchId, ct);
        int success = txns.Count(t => t.TxnStatus == "COMPLETED");
        int failure = txns.Count(t => t.TxnStatus == "FAILED");
        decimal total = txns.Where(t => t.TxnStatus == "COMPLETED").Sum(t => t.TxnBaseAmount ?? 0m);

        batch.Complete(success, failure, total);
        await uow.SaveChangesAsync(ct);

        foreach (var evt in batch.DomainEvents)
            await publisher.PublishAsync(evt, "transaction.batch.completed", ct);

        batch.ClearDomainEvents();

        return new TransactionBatchDto(
            batch.BatchId, batch.BatchType, batch.BatchDate, batch.BatchStatus,
            batch.BatchTotalCount, batch.BatchSuccessCount, batch.BatchFailureCount,
            batch.BatchTotalAmount, batch.CreatedBy, batch.CreatedOn, batch.CompletedOn);
    }
}
