using ContributionService.Domain.Interfaces;
using ContributionService.Infrastructure.Persistence;

namespace ContributionService.Infrastructure.Repositories;

public class UnitOfWork(ContributionDbContext context) : IUnitOfWork
{
    private IContributionMainRepository? _contributionMain;
    private IContributionDetailRepository? _contributionDetails;
    private IContributionBreakupRepository? _contributionBreakups;
    private ISuperannuationBatchRepository? _superannuationBatches;
    private ISuperannuationContributionRepository? _superannuationContributions;
    private ISuperannuationTrustNameRepository? _superannuationTrustNames;
    private IContributionProcessLogRepository? _processLogs;

    public IContributionMainRepository ContributionMain
        => _contributionMain ??= new ContributionMainRepository(context);

    public IContributionDetailRepository ContributionDetails
        => _contributionDetails ??= new ContributionDetailRepository(context);

    public IContributionBreakupRepository ContributionBreakups
        => _contributionBreakups ??= new ContributionBreakupRepository(context);

    public ISuperannuationBatchRepository SuperannuationBatches
        => _superannuationBatches ??= new SuperannuationBatchRepository(context);

    public ISuperannuationContributionRepository SuperannuationContributions
        => _superannuationContributions ??= new SuperannuationContributionRepository(context);

    public ISuperannuationTrustNameRepository SuperannuationTrustNames
        => _superannuationTrustNames ??= new SuperannuationTrustNameRepository(context);

    public IContributionProcessLogRepository ProcessLogs
        => _processLogs ??= new ContributionProcessLogRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
