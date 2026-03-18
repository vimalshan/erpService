using ContributionService.Domain.Entities;

namespace ContributionService.Domain.Interfaces;

public interface IContributionMainRepository
{
    Task<ContributionMain?> GetByIdAsync(long batchNo, CancellationToken ct = default);
    Task<IReadOnlyList<ContributionMain>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ContributionMain>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<ContributionMain>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default);
    Task<ContributionMain> AddAsync(ContributionMain entity, CancellationToken ct = default);
    Task UpdateAsync(ContributionMain entity, CancellationToken ct = default);
    Task<long> GetNextBatchNoAsync(CancellationToken ct = default);
}

public interface IContributionDetailRepository
{
    Task<ContributionDetail?> GetByIdAsync(decimal id, CancellationToken ct = default);
    Task<IReadOnlyList<ContributionDetail>> GetByBatchNoAsync(decimal batchNo, CancellationToken ct = default);
    Task<IReadOnlyList<ContributionDetail>> GetByMemberNoAsync(decimal memberNo, CancellationToken ct = default);
    Task<ContributionDetail> AddAsync(ContributionDetail entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<ContributionDetail> entities, CancellationToken ct = default);
    Task UpdateAsync(ContributionDetail entity, CancellationToken ct = default);
}

public interface IContributionBreakupRepository
{
    Task<IReadOnlyList<ContributionBreakup>> GetByBatchAndIdAsync(long batchNo, long id, CancellationToken ct = default);
    Task AddAsync(ContributionBreakup entity, CancellationToken ct = default);
}

public interface ISuperannuationBatchRepository
{
    Task<SuperannuationBatch?> GetByIdAsync(long batchNo, CancellationToken ct = default);
    Task<IReadOnlyList<SuperannuationBatch>> GetAllAsync(CancellationToken ct = default);
    Task<SuperannuationBatch> AddAsync(SuperannuationBatch entity, CancellationToken ct = default);
    Task UpdateAsync(SuperannuationBatch entity, CancellationToken ct = default);
}

public interface ISuperannuationContributionRepository
{
    Task<SuperannuationContribution?> GetByIdAsync(long slrNum, CancellationToken ct = default);
    Task<IReadOnlyList<SuperannuationContribution>> GetByFundAsync(decimal fundNum, CancellationToken ct = default);
    Task<SuperannuationContribution> AddAsync(SuperannuationContribution entity, CancellationToken ct = default);
}

public interface ISuperannuationTrustNameRepository
{
    Task<IReadOnlyList<SuperannuationTrustName>> GetAllAsync(CancellationToken ct = default);
    Task<SuperannuationTrustName?> GetByIdAsync(decimal fundNum, CancellationToken ct = default);
}

public interface IContributionProcessLogRepository
{
    Task AddAsync(ContributionProcessLog log, CancellationToken ct = default);
    Task<IReadOnlyList<ContributionProcessLog>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IContributionMainRepository ContributionMain { get; }
    IContributionDetailRepository ContributionDetails { get; }
    IContributionBreakupRepository ContributionBreakups { get; }
    ISuperannuationBatchRepository SuperannuationBatches { get; }
    ISuperannuationContributionRepository SuperannuationContributions { get; }
    ISuperannuationTrustNameRepository SuperannuationTrustNames { get; }
    IContributionProcessLogRepository ProcessLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
