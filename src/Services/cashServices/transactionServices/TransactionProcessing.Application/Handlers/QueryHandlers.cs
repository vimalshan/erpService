using MediatR;
using TransactionProcessing.Application.DTOs;
using TransactionProcessing.Application.Queries;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Application.Handlers;

public sealed class GetTransactionByIdHandler(IUnitOfWork uow)
    : IRequestHandler<GetTransactionByIdQuery, FinancialTransactionDto?>
{
    public async Task<FinancialTransactionDto?> Handle(GetTransactionByIdQuery query, CancellationToken ct)
    {
        var txn = await uow.Transactions.GetByIdAsync(query.TxnId, ct);
        if (txn is null) return null;

        return new FinancialTransactionDto(
            txn.TxnId, txn.TxnBatchId, txn.TxnType, txn.TxnSubType,
            txn.TxnAmount, txn.TxnCurrencyId, txn.TxnExchangeRate, txn.TxnBaseAmount,
            txn.TxnReference, txn.TxnSourceService, txn.TxnSourceId,
            txn.TxnStatus, txn.TxnRemarks, txn.CreatedBy, txn.CreatedOn);
    }
}

public sealed class GetTransactionsByBatchHandler(IUnitOfWork uow)
    : IRequestHandler<GetTransactionsByBatchQuery, IReadOnlyList<FinancialTransactionDto>>
{
    public async Task<IReadOnlyList<FinancialTransactionDto>> Handle(GetTransactionsByBatchQuery query, CancellationToken ct)
    {
        var txns = await uow.Transactions.GetByBatchIdAsync(query.BatchId, ct);
        return txns.Select(t => new FinancialTransactionDto(
            t.TxnId, t.TxnBatchId, t.TxnType, t.TxnSubType,
            t.TxnAmount, t.TxnCurrencyId, t.TxnExchangeRate, t.TxnBaseAmount,
            t.TxnReference, t.TxnSourceService, t.TxnSourceId,
            t.TxnStatus, t.TxnRemarks, t.CreatedBy, t.CreatedOn)).ToList();
    }
}

public sealed class GetSettlementsByDealHandler(IUnitOfWork uow)
    : IRequestHandler<GetSettlementsByDealQuery, IReadOnlyList<DealSettlementDto>>
{
    public async Task<IReadOnlyList<DealSettlementDto>> Handle(GetSettlementsByDealQuery query, CancellationToken ct)
    {
        var items = await uow.DealSettlements.GetByDealIdAsync(query.DealId, ct);
        return items.Select(s => new DealSettlementDto(
            s.SettlementId, s.TxnId, s.DealId, s.SetId, s.SettlementType.ToString(),
            s.SpotRate, s.ExchangeRate, s.SettlementAmount, s.GainLossAmount,
            s.PremiumAmount, s.WindingFee, s.NetAmount, s.BankAccountId,
            s.ProcessingStatus, s.CreatedOn)).ToList();
    }
}

public sealed class GetDisbursementsByLoanHandler(IUnitOfWork uow)
    : IRequestHandler<GetDisbursementsByLoanQuery, IReadOnlyList<LoanDisbursementDto>>
{
    public async Task<IReadOnlyList<LoanDisbursementDto>> Handle(GetDisbursementsByLoanQuery query, CancellationToken ct)
    {
        var items = await uow.LoanDisbursements.GetByLoanIdAsync(query.LoanId, ct);
        return items.Select(d => new LoanDisbursementDto(
            d.DisbProcId, d.TxnId, d.LoanId, d.DisbId, d.DisbAmount,
            d.ExchangeRate, d.ConvertedAmount, d.BankAccountId,
            d.ProcessingStatus, d.CreatedOn)).ToList();
    }
}

public sealed class GetRepaymentsByLoanHandler(IUnitOfWork uow)
    : IRequestHandler<GetRepaymentsByLoanQuery, IReadOnlyList<LoanRepaymentDto>>
{
    public async Task<IReadOnlyList<LoanRepaymentDto>> Handle(GetRepaymentsByLoanQuery query, CancellationToken ct)
    {
        var items = await uow.LoanRepayments.GetByLoanIdAsync(query.LoanId, ct);
        return items.Select(r => new LoanRepaymentDto(
            r.RepayProcId, r.TxnId, r.LoanId, r.RepayId, r.RepayAmount,
            r.ExchangeRate, r.ConvertedAmount, r.BankAccountId,
            r.ProcessingStatus, r.CreatedOn)).ToList();
    }
}

public sealed class GetTransactionLedgerHandler(IUnitOfWork uow)
    : IRequestHandler<GetTransactionLedgerQuery, IReadOnlyList<TransactionLedgerDto>>
{
    public async Task<IReadOnlyList<TransactionLedgerDto>> Handle(GetTransactionLedgerQuery query, CancellationToken ct)
    {
        var from = query.From ?? DateTime.UtcNow.AddDays(-30);
        var to = query.To ?? DateTime.UtcNow;
        var txns = await uow.Transactions.GetByDateRangeAsync(from, to, ct);

        var filtered = txns.AsEnumerable();
        if (!string.IsNullOrEmpty(query.Status)) filtered = filtered.Where(t => t.TxnStatus == query.Status);
        if (!string.IsNullOrEmpty(query.TxnType)) filtered = filtered.Where(t => t.TxnType == query.TxnType);

        var paged = filtered.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList();

        var result = new List<TransactionLedgerDto>();
        foreach (var txn in paged)
        {
            var audits = await uow.TransactionAudits.GetByTxnIdAsync(txn.TxnId, ct);
            result.Add(new TransactionLedgerDto(
                txn.TxnId, txn.TxnType, txn.TxnReference,
                txn.TxnAmount, txn.TxnCurrencyId, txn.TxnBaseAmount,
                txn.TxnStatus, txn.TxnSourceService, txn.CreatedOn,
                audits.Select(a => new TransactionAuditDto(
                    a.AuditId, a.PreviousStatus, a.NewStatus,
                    a.AuditAction, a.AuditRemarks, a.AuditBy, a.AuditOn)).ToList()));
        }
        return result;
    }
}
