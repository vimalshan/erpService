using TransactionProcessing.Application.DTOs;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.API.GraphQL;

public sealed class Query
{
    [GraphQLDescription("Get a transaction by ID")]
    public async Task<FinancialTransactionDto?> GetTransaction(long txnId, [Service] IUnitOfWork uow, CancellationToken ct)
    {
        var txn = await uow.Transactions.GetByIdAsync(txnId, ct);
        if (txn is null) return null;
        return new FinancialTransactionDto(
            txn.TxnId, txn.TxnBatchId, txn.TxnType, txn.TxnSubType,
            txn.TxnAmount, txn.TxnCurrencyId, txn.TxnExchangeRate, txn.TxnBaseAmount,
            txn.TxnReference, txn.TxnSourceService, txn.TxnSourceId,
            txn.TxnStatus, txn.TxnRemarks, txn.CreatedBy, txn.CreatedOn);
    }

    [GraphQLDescription("Get settlements for a deal")]
    public async Task<IReadOnlyList<DealSettlementDto>> GetSettlementsByDeal(long dealId, [Service] IUnitOfWork uow, CancellationToken ct)
    {
        var items = await uow.DealSettlements.GetByDealIdAsync(dealId, ct);
        return items.Select(s => new DealSettlementDto(
            s.SettlementId, s.TxnId, s.DealId, s.SetId, s.SettlementType.ToString(),
            s.SpotRate, s.ExchangeRate, s.SettlementAmount, s.GainLossAmount,
            s.PremiumAmount, s.WindingFee, s.NetAmount, s.BankAccountId,
            s.ProcessingStatus, s.CreatedOn)).ToList();
    }

    [GraphQLDescription("Get disbursements for a loan")]
    public async Task<IReadOnlyList<LoanDisbursementDto>> GetDisbursementsByLoan(long loanId, [Service] IUnitOfWork uow, CancellationToken ct)
    {
        var items = await uow.LoanDisbursements.GetByLoanIdAsync(loanId, ct);
        return items.Select(d => new LoanDisbursementDto(
            d.DisbProcId, d.TxnId, d.LoanId, d.DisbId, d.DisbAmount,
            d.ExchangeRate, d.ConvertedAmount, d.BankAccountId,
            d.ProcessingStatus, d.CreatedOn)).ToList();
    }

    [GraphQLDescription("Get repayments for a loan")]
    public async Task<IReadOnlyList<LoanRepaymentDto>> GetRepaymentsByLoan(long loanId, [Service] IUnitOfWork uow, CancellationToken ct)
    {
        var items = await uow.LoanRepayments.GetByLoanIdAsync(loanId, ct);
        return items.Select(r => new LoanRepaymentDto(
            r.RepayProcId, r.TxnId, r.LoanId, r.RepayId, r.RepayAmount,
            r.ExchangeRate, r.ConvertedAmount, r.BankAccountId,
            r.ProcessingStatus, r.CreatedOn)).ToList();
    }

    [GraphQLDescription("Get transaction batches by status")]
    public async Task<IReadOnlyList<TransactionBatchDto>> GetBatchesByStatus(string status, [Service] IUnitOfWork uow, CancellationToken ct)
    {
        var items = await uow.Batches.GetByStatusAsync(status, ct);
        return items.Select(b => new TransactionBatchDto(
            b.BatchId, b.BatchType, b.BatchDate, b.BatchStatus,
            b.BatchTotalCount, b.BatchSuccessCount, b.BatchFailureCount,
            b.BatchTotalAmount, b.CreatedBy, b.CreatedOn, b.CompletedOn)).ToList();
    }
}
