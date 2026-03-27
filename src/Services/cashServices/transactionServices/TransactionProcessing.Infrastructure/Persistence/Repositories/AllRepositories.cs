using Microsoft.EntityFrameworkCore;
using TransactionProcessing.Domain.Entities;
using TransactionProcessing.Domain.Interfaces;
using TransactionProcessing.Infrastructure.Persistence;

namespace TransactionProcessing.Infrastructure.Persistence.Repositories;

public sealed class FinancialTransactionRepository(TransactionDbContext db) : IFinancialTransactionRepository
{
    public async Task<FinancialTransaction?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.FinancialTransactions.Include(t => t.Audits).FirstOrDefaultAsync(t => t.TxnId == id, ct);

    public async Task<IReadOnlyList<FinancialTransaction>> GetByBatchIdAsync(long batchId, CancellationToken ct) =>
        await db.FinancialTransactions.Where(t => t.TxnBatchId == batchId).ToListAsync(ct);

    public async Task<IReadOnlyList<FinancialTransaction>> GetBySourceAsync(string sourceService, long? sourceId, CancellationToken ct)
    {
        var query = db.FinancialTransactions.Where(t => t.TxnSourceService == sourceService);
        if (sourceId.HasValue) query = query.Where(t => t.TxnSourceId == sourceId.Value);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<FinancialTransaction>> GetByStatusAsync(string status, CancellationToken ct) =>
        await db.FinancialTransactions.Where(t => t.TxnStatus == status).ToListAsync(ct);

    public async Task<IReadOnlyList<FinancialTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct) =>
        await db.FinancialTransactions.Where(t => t.CreatedOn >= from && t.CreatedOn <= to).OrderByDescending(t => t.CreatedOn).ToListAsync(ct);

    public async Task AddAsync(FinancialTransaction transaction, CancellationToken ct) =>
        await db.FinancialTransactions.AddAsync(transaction, ct);

    public void Update(FinancialTransaction transaction) => db.FinancialTransactions.Update(transaction);
}

public sealed class TransactionBatchRepository(TransactionDbContext db) : ITransactionBatchRepository
{
    public async Task<TransactionBatch?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.TransactionBatches.Include(b => b.Transactions).FirstOrDefaultAsync(b => b.BatchId == id, ct);

    public async Task<IReadOnlyList<TransactionBatch>> GetByStatusAsync(string status, CancellationToken ct) =>
        await db.TransactionBatches.Where(b => b.BatchStatus == status).ToListAsync(ct);

    public async Task<IReadOnlyList<TransactionBatch>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct) =>
        await db.TransactionBatches.Where(b => b.BatchDate >= from && b.BatchDate <= to).OrderByDescending(b => b.BatchDate).ToListAsync(ct);

    public async Task AddAsync(TransactionBatch batch, CancellationToken ct) =>
        await db.TransactionBatches.AddAsync(batch, ct);

    public void Update(TransactionBatch batch) => db.TransactionBatches.Update(batch);
}

public sealed class DealSettlementRepository(TransactionDbContext db) : IDealSettlementRepository
{
    public async Task<DealSettlement?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.DealSettlements.FirstOrDefaultAsync(s => s.SettlementId == id, ct);

    public async Task<IReadOnlyList<DealSettlement>> GetByDealIdAsync(long dealId, CancellationToken ct) =>
        await db.DealSettlements.Where(s => s.DealId == dealId).ToListAsync(ct);

    public async Task<IReadOnlyList<DealSettlement>> GetByTxnIdAsync(long txnId, CancellationToken ct) =>
        await db.DealSettlements.Where(s => s.TxnId == txnId).ToListAsync(ct);

    public async Task AddAsync(DealSettlement settlement, CancellationToken ct) =>
        await db.DealSettlements.AddAsync(settlement, ct);

    public void Update(DealSettlement settlement) => db.DealSettlements.Update(settlement);
}

public sealed class LoanDisbursementRepository(TransactionDbContext db) : ILoanDisbursementRepository
{
    public async Task<LoanDisbursement?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.LoanDisbursements.FirstOrDefaultAsync(d => d.DisbProcId == id, ct);

    public async Task<IReadOnlyList<LoanDisbursement>> GetByLoanIdAsync(long loanId, CancellationToken ct) =>
        await db.LoanDisbursements.Where(d => d.LoanId == loanId).ToListAsync(ct);

    public async Task AddAsync(LoanDisbursement disbursement, CancellationToken ct) =>
        await db.LoanDisbursements.AddAsync(disbursement, ct);

    public void Update(LoanDisbursement disbursement) => db.LoanDisbursements.Update(disbursement);
}

public sealed class LoanRepaymentRepository(TransactionDbContext db) : ILoanRepaymentRepository
{
    public async Task<LoanRepayment?> GetByIdAsync(long id, CancellationToken ct) =>
        await db.LoanRepayments.FirstOrDefaultAsync(r => r.RepayProcId == id, ct);

    public async Task<IReadOnlyList<LoanRepayment>> GetByLoanIdAsync(long loanId, CancellationToken ct) =>
        await db.LoanRepayments.Where(r => r.LoanId == loanId).ToListAsync(ct);

    public async Task AddAsync(LoanRepayment repayment, CancellationToken ct) =>
        await db.LoanRepayments.AddAsync(repayment, ct);

    public void Update(LoanRepayment repayment) => db.LoanRepayments.Update(repayment);
}

public sealed class TransactionAuditRepository(TransactionDbContext db) : ITransactionAuditRepository
{
    public async Task<IReadOnlyList<TransactionAudit>> GetByTxnIdAsync(long txnId, CancellationToken ct) =>
        await db.TransactionAudits.Where(a => a.TxnId == txnId).OrderBy(a => a.AuditOn).ToListAsync(ct);

    public async Task AddAsync(TransactionAudit audit, CancellationToken ct) =>
        await db.TransactionAudits.AddAsync(audit, ct);
}

public sealed class UnitOfWork(
    TransactionDbContext context,
    IFinancialTransactionRepository transactions,
    ITransactionBatchRepository batches,
    IDealSettlementRepository dealSettlements,
    ILoanDisbursementRepository loanDisbursements,
    ILoanRepaymentRepository loanRepayments,
    ITransactionAuditRepository transactionAudits) : IUnitOfWork
{
    public IFinancialTransactionRepository Transactions => transactions;
    public ITransactionBatchRepository Batches => batches;
    public IDealSettlementRepository DealSettlements => dealSettlements;
    public ILoanDisbursementRepository LoanDisbursements => loanDisbursements;
    public ILoanRepaymentRepository LoanRepayments => loanRepayments;
    public ITransactionAuditRepository TransactionAudits => transactionAudits;

    public async Task<int> SaveChangesAsync(CancellationToken ct) => await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
