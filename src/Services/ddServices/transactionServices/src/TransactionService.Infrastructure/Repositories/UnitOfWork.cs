using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Data.TransactionDbContext _context;
    private IDemandMasterRepository? _demandMasterRepository;
    private ISaaBudgetRepository? _saaBudgetRepository;
    private ISaaPeriodRepository? _saaPeriodRepository;
    private ISaaLevelRepository? _saaLevelRepository;
    private ISaaRecommendRepository? _saaRecommendRepository;
    private ISaaSubmitRepository? _saaSubmitRepository;
    private ISaaMailTriggerRepository? _saaMailTriggerRepository;

    public UnitOfWork(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public IDemandMasterRepository DemandMasters =>
        _demandMasterRepository ??= new DemandMasterRepository(_context);

    public ISaaBudgetRepository SaaBudgets =>
        _saaBudgetRepository ??= new SaaBudgetRepository(_context);

    public ISaaPeriodRepository SaaPeriods =>
        _saaPeriodRepository ??= new SaaPeriodRepository(_context);

    public ISaaLevelRepository SaaLevels =>
        _saaLevelRepository ??= new SaaLevelRepository(_context);

    public ISaaRecommendRepository SaaRecommends =>
        _saaRecommendRepository ??= new SaaRecommendRepository(_context);

    public ISaaSubmitRepository SaaSubmits =>
        _saaSubmitRepository ??= new SaaSubmitRepository(_context);

    public ISaaMailTriggerRepository SaaMailTriggers =>
        _saaMailTriggerRepository ??= new SaaMailTriggerRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error rolling back transaction: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
