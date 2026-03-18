using ReportingService.Domain.Entities;
using ReportingService.Domain.Interfaces;

namespace ReportingService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Data.ReportingDbContext _context;
    private IAppraisalRepository? _appraisalRepository;
    private IRepository<AppraisalGoal>? _appraisalGoalRepository;
    private IRepository<AppraiseePerformance>? _appraiseePerformanceRepository;
    private IRepository<DDRating>? _ddRatingRepository;

    public UnitOfWork(Data.ReportingDbContext context)
    {
        _context = context;
    }

    public IAppraisalRepository Appraisals =>
        _appraisalRepository ??= new AppraisalRepository(_context);

    public IRepository<AppraisalGoal> AppraisalGoals =>
        _appraisalGoalRepository ??= new GenericRepository<AppraisalGoal>(_context);

    public IRepository<AppraiseePerformance> AppraiseePerformances =>
        _appraiseePerformanceRepository ??= new GenericRepository<AppraiseePerformance>(_context);

    public IRepository<DDRating> DDRatings =>
        _ddRatingRepository ??= new GenericRepository<DDRating>(_context);

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

    public async Task RollbackAsync()
    {
        try
        {
            await _context.Database.RollbackTransactionAsync();
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
