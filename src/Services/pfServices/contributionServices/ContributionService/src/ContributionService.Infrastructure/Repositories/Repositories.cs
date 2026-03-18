using ContributionService.Domain.Entities;
using ContributionService.Domain.Interfaces;
using ContributionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContributionService.Infrastructure.Repositories;

public class ContributionMainRepository(ContributionDbContext context) : IContributionMainRepository
{
    public async Task<ContributionMain?> GetByIdAsync(long batchNo, CancellationToken ct = default)
        => await context.ContributionMain.Include(x => x.Details).FirstOrDefaultAsync(x => x.ContributionBatchNo == batchNo, ct);

    public async Task<IReadOnlyList<ContributionMain>> GetAllAsync(CancellationToken ct = default)
        => await context.ContributionMain.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<ContributionMain>> GetByStatusAsync(string status, CancellationToken ct = default)
        => await context.ContributionMain.AsNoTracking().Where(x => x.ContributionStatus == status).ToListAsync(ct);

    public async Task<IReadOnlyList<ContributionMain>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default)
        => await context.ContributionMain.AsNoTracking()
            .Where(x => x.ContributionPayMonthStart >= start && x.ContributionPayMonthEnd <= end).ToListAsync(ct);

    public async Task<ContributionMain> AddAsync(ContributionMain entity, CancellationToken ct = default)
    {
        await context.ContributionMain.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(ContributionMain entity, CancellationToken ct = default)
    {
        context.ContributionMain.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextBatchNoAsync(CancellationToken ct = default)
    {
        var max = await context.ContributionMain.MaxAsync(x => (long?)x.ContributionBatchNo, ct);
        return (max ?? 0) + 1;
    }
}

public class ContributionDetailRepository(ContributionDbContext context) : IContributionDetailRepository
{
    public async Task<ContributionDetail?> GetByIdAsync(decimal id, CancellationToken ct = default)
        => await context.ContributionDetails.FirstOrDefaultAsync(x => x.ContributionId == id, ct);

    public async Task<IReadOnlyList<ContributionDetail>> GetByBatchNoAsync(decimal batchNo, CancellationToken ct = default)
        => await context.ContributionDetails.AsNoTracking().Where(x => x.ContributionBatchNo == batchNo).ToListAsync(ct);

    public async Task<IReadOnlyList<ContributionDetail>> GetByMemberNoAsync(decimal memberNo, CancellationToken ct = default)
        => await context.ContributionDetails.AsNoTracking().Where(x => x.ContributionMemberNo == memberNo).ToListAsync(ct);

    public async Task<ContributionDetail> AddAsync(ContributionDetail entity, CancellationToken ct = default)
    {
        await context.ContributionDetails.AddAsync(entity, ct);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ContributionDetail> entities, CancellationToken ct = default)
        => await context.ContributionDetails.AddRangeAsync(entities, ct);

    public Task UpdateAsync(ContributionDetail entity, CancellationToken ct = default)
    {
        context.ContributionDetails.Update(entity);
        return Task.CompletedTask;
    }
}

public class ContributionBreakupRepository(ContributionDbContext context) : IContributionBreakupRepository
{
    public async Task<IReadOnlyList<ContributionBreakup>> GetByBatchAndIdAsync(long batchNo, long id, CancellationToken ct = default)
        => await context.ContributionBreakups.AsNoTracking()
            .Where(x => x.ContributionBatchNo == batchNo && x.ContributionId == id).ToListAsync(ct);

    public async Task AddAsync(ContributionBreakup entity, CancellationToken ct = default)
        => await context.ContributionBreakups.AddAsync(entity, ct);
}

public class SuperannuationBatchRepository(ContributionDbContext context) : ISuperannuationBatchRepository
{
    public async Task<SuperannuationBatch?> GetByIdAsync(long batchNo, CancellationToken ct = default)
        => await context.SuperannuationBatches.FirstOrDefaultAsync(x => x.SnBatchNo == batchNo, ct);

    public async Task<IReadOnlyList<SuperannuationBatch>> GetAllAsync(CancellationToken ct = default)
        => await context.SuperannuationBatches.AsNoTracking().ToListAsync(ct);

    public async Task<SuperannuationBatch> AddAsync(SuperannuationBatch entity, CancellationToken ct = default)
    {
        await context.SuperannuationBatches.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(SuperannuationBatch entity, CancellationToken ct = default)
    {
        context.SuperannuationBatches.Update(entity);
        return Task.CompletedTask;
    }
}

public class SuperannuationContributionRepository(ContributionDbContext context) : ISuperannuationContributionRepository
{
    public async Task<SuperannuationContribution?> GetByIdAsync(long slrNum, CancellationToken ct = default)
        => await context.SuperannuationContributions.FirstOrDefaultAsync(x => x.SnSlrNum == slrNum, ct);

    public async Task<IReadOnlyList<SuperannuationContribution>> GetByFundAsync(decimal fundNum, CancellationToken ct = default)
        => await context.SuperannuationContributions.AsNoTracking().Where(x => x.SnFudNum == fundNum).ToListAsync(ct);

    public async Task<SuperannuationContribution> AddAsync(SuperannuationContribution entity, CancellationToken ct = default)
    {
        await context.SuperannuationContributions.AddAsync(entity, ct);
        return entity;
    }
}

public class SuperannuationTrustNameRepository(ContributionDbContext context) : ISuperannuationTrustNameRepository
{
    public async Task<IReadOnlyList<SuperannuationTrustName>> GetAllAsync(CancellationToken ct = default)
        => await context.SuperannuationTrustNames.AsNoTracking().ToListAsync(ct);

    public async Task<SuperannuationTrustName?> GetByIdAsync(decimal fundNum, CancellationToken ct = default)
        => await context.SuperannuationTrustNames.FirstOrDefaultAsync(x => x.StFndNum == fundNum, ct);
}

public class ContributionProcessLogRepository(ContributionDbContext context) : IContributionProcessLogRepository
{
    public async Task AddAsync(ContributionProcessLog log, CancellationToken ct = default)
        => await context.ContributionProcessLogs.AddAsync(log, ct);

    public async Task<IReadOnlyList<ContributionProcessLog>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default)
        => await context.ContributionProcessLogs.AsNoTracking()
            .Where(x => x.ProcessDate >= start && x.ProcessDate <= end).ToListAsync(ct);
}
