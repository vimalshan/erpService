using TransactionProcessing.Domain.Entities;

namespace TransactionProcessing.Domain.Interfaces;

public interface IFinancialTransactionRepository
{
    Task<FinancialTransaction?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<FinancialTransaction>> GetByBatchIdAsync(long batchId, CancellationToken ct = default);
    Task<IReadOnlyList<FinancialTransaction>> GetBySourceAsync(string sourceService, long? sourceId, CancellationToken ct = default);
    Task<IReadOnlyList<FinancialTransaction>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<FinancialTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(FinancialTransaction transaction, CancellationToken ct = default);
    void Update(FinancialTransaction transaction);
}

public interface ITransactionBatchRepository
{
    Task<TransactionBatch?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<TransactionBatch>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<TransactionBatch>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(TransactionBatch batch, CancellationToken ct = default);
    void Update(TransactionBatch batch);
}

public interface IDealSettlementRepository
{
    Task<DealSettlement?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<DealSettlement>> GetByDealIdAsync(long dealId, CancellationToken ct = default);
    Task<IReadOnlyList<DealSettlement>> GetByTxnIdAsync(long txnId, CancellationToken ct = default);
    Task AddAsync(DealSettlement settlement, CancellationToken ct = default);
    void Update(DealSettlement settlement);
}

public interface ILoanDisbursementRepository
{
    Task<LoanDisbursement?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<LoanDisbursement>> GetByLoanIdAsync(long loanId, CancellationToken ct = default);
    Task AddAsync(LoanDisbursement disbursement, CancellationToken ct = default);
    void Update(LoanDisbursement disbursement);
}

public interface ILoanRepaymentRepository
{
    Task<LoanRepayment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<LoanRepayment>> GetByLoanIdAsync(long loanId, CancellationToken ct = default);
    Task AddAsync(LoanRepayment repayment, CancellationToken ct = default);
    void Update(LoanRepayment repayment);
}

public interface ITransactionAuditRepository
{
    Task<IReadOnlyList<TransactionAudit>> GetByTxnIdAsync(long txnId, CancellationToken ct = default);
    Task AddAsync(TransactionAudit audit, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IFinancialTransactionRepository Transactions { get; }
    ITransactionBatchRepository Batches { get; }
    IDealSettlementRepository DealSettlements { get; }
    ILoanDisbursementRepository LoanDisbursements { get; }
    ILoanRepaymentRepository LoanRepayments { get; }
    ITransactionAuditRepository TransactionAudits { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : class;
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<IReadOnlyList<string>> ListAsync(string containerName, string? prefix = null, CancellationToken ct = default);
}
