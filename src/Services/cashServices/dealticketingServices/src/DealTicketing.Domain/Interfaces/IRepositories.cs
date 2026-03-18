using DealTicketing.Domain.Entities;

namespace DealTicketing.Domain.Interfaces;

public interface IDealBatchRepository
{
    Task<DealBatch?> GetByIdAsync(long batchId, CancellationToken ct = default);
    Task<IReadOnlyList<DealBatch>> GetByDateAsync(DateTime date, CancellationToken ct = default);
    Task AddAsync(DealBatch batch, CancellationToken ct = default);
    void Update(DealBatch batch);
    Task<bool> ExistsAsync(long batchId, CancellationToken ct = default);
}

public interface IDealDetailRepository
{
    Task<DealDetail?> GetByIdAsync(long dealId, CancellationToken ct = default);
    Task<IReadOnlyList<DealDetail>> GetByBatchIdAsync(long batchId, CancellationToken ct = default);
    Task<IReadOnlyList<DealDetail>> GetPendingApprovalsAsync(CancellationToken ct = default);
    Task AddAsync(DealDetail deal, CancellationToken ct = default);
    void Update(DealDetail deal);
}

public interface IDealSettlementRepository
{
    Task<DealSettlement?> GetByIdAsync(long setId, CancellationToken ct = default);
    Task<IReadOnlyList<DealSettlement>> GetByDealIdAsync(long dealId, CancellationToken ct = default);
    Task AddAsync(DealSettlement settlement, CancellationToken ct = default);
    void Update(DealSettlement settlement);
}

public interface IBankRepository
{
    Task<Bank?> GetByIdAsync(long bankId, CancellationToken ct = default);
    Task<IReadOnlyList<Bank>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(Bank bank, CancellationToken ct = default);
    void Update(Bank bank);
}
