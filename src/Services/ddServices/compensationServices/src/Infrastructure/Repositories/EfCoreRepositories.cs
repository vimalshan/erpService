namespace CompensationService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using CompensationService.Domain.Entities;
using CompensationService.Domain.Repositories;
using CompensationService.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for budget operations using Entity Framework Core.
/// </summary>
public class BudgetRepository : IBudgetRepository
{
    private readonly CompensationDbContext _context;

    public BudgetRepository(CompensationDbContext context)
    {
        _context = context;
    }

    public async Task<Budget?> GetByIdAsync(decimal budgetId, CancellationToken cancellationToken = default)
    {
        return await _context.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);
    }

    public async Task<IEnumerable<Budget>> GetByYearAndBusinessAsync(decimal yearId, decimal businessId, CancellationToken cancellationToken = default)
    {
        return await _context.Budgets
            .Where(b => b.YearId == yearId && b.BusinessId == businessId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Budget budget, CancellationToken cancellationToken = default)
    {
        await _context.Budgets.AddAsync(budget, cancellationToken);
    }

    public async Task UpdateAsync(Budget budget, CancellationToken cancellationToken = default)
    {
        _context.Budgets.Update(budget);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for compensation level operations using Entity Framework Core.
/// </summary>
public class CompensationLevelRepository : ICompensationLevelRepository
{
    private readonly CompensationDbContext _context;

    public CompensationLevelRepository(CompensationDbContext context)
    {
        _context = context;
    }

    public async Task<CompensationLevel?> GetByIdAsync(decimal levelId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationLevels.FirstOrDefaultAsync(l => l.Id == levelId, cancellationToken);
    }

    public async Task<IEnumerable<CompensationLevel>> GetActiveLevelsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompensationLevels
            .Where(l => l.CloseDate == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompensationLevel>> GetAllLevelsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompensationLevels.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CompensationLevel level, CancellationToken cancellationToken = default)
    {
        await _context.CompensationLevels.AddAsync(level, cancellationToken);
    }

    public async Task UpdateAsync(CompensationLevel level, CancellationToken cancellationToken = default)
    {
        _context.CompensationLevels.Update(level);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for compensation period operations using Entity Framework Core.
/// </summary>
public class CompensationPeriodRepository : ICompensationPeriodRepository
{
    private readonly CompensationDbContext _context;

    public CompensationPeriodRepository(CompensationDbContext context)
    {
        _context = context;
    }

    public async Task<CompensationPeriod?> GetByIdAsync(decimal periodId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationPeriods.FirstOrDefaultAsync(p => p.Id == periodId, cancellationToken);
    }

    public async Task<IEnumerable<CompensationPeriod>> GetByYearAsync(decimal yearId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationPeriods
            .Where(p => p.YearId == yearId)
            .OrderBy(p => p.QuarterNo)
            .ToListAsync(cancellationToken);
    }

    public async Task<CompensationPeriod?> GetByYearAndQuarterAsync(decimal yearId, decimal quarterNo, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationPeriods
            .FirstOrDefaultAsync(p => p.YearId == yearId && p.QuarterNo == quarterNo, cancellationToken);
    }

    public async Task<IEnumerable<CompensationPeriod>> GetOpenPeriodsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CompensationPeriods
            .Where(p => p.Status.StatusCode == "O")
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CompensationPeriod period, CancellationToken cancellationToken = default)
    {
        await _context.CompensationPeriods.AddAsync(period, cancellationToken);
    }

    public async Task UpdateAsync(CompensationPeriod period, CancellationToken cancellationToken = default)
    {
        _context.CompensationPeriods.Update(period);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for compensation recommendation operations using Entity Framework Core.
/// </summary>
public class CompensationRecommendationRepository : ICompensationRecommendationRepository
{
    private readonly CompensationDbContext _context;

    public CompensationRecommendationRepository(CompensationDbContext context)
    {
        _context = context;
    }

    public async Task<CompensationRecommendation?> GetByIdAsync(decimal recommendationId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationRecommendations
            .FirstOrDefaultAsync(r => r.Id == recommendationId, cancellationToken);
    }

    public async Task<IEnumerable<CompensationRecommendation>> GetByPeriodAndEmployeeAsync(decimal periodId, decimal employeeSystemId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationRecommendations
            .Where(r => r.PeriodId == periodId && r.EmployeeSystemId == employeeSystemId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompensationRecommendation>> GetByPeriodAsync(decimal periodId, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationRecommendations
            .Where(r => r.PeriodId == periodId)
            .OrderByDescending(r => r.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompensationRecommendation>> GetByStatusAsync(int statusCode, CancellationToken cancellationToken = default)
    {
        return await _context.CompensationRecommendations
            .Where(r => r.Status.StatusCode == statusCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompensationRecommendation>> GetPendingForReviewerAsync(decimal periodId, string role, CancellationToken cancellationToken = default)
    {
        return role.ToUpper() switch
        {
            "REV" => await _context.CompensationRecommendations
                .Where(r => r.PeriodId == periodId && r.Status.StatusCode == 2) // AppraisalSubmitted
                .ToListAsync(cancellationToken),
            "BHR" => await _context.CompensationRecommendations
                .Where(r => r.PeriodId == periodId && r.Status.StatusCode == 3) // ReviewerSubmitted
                .ToListAsync(cancellationToken),
            "CHR" => await _context.CompensationRecommendations
                .Where(r => r.PeriodId == periodId && r.Status.StatusCode == 4) // BhrSubmitted
                .ToListAsync(cancellationToken),
            _ => new List<CompensationRecommendation>()
        };
    }

    public async Task AddAsync(CompensationRecommendation recommendation, CancellationToken cancellationToken = default)
    {
        await _context.CompensationRecommendations.AddAsync(recommendation, cancellationToken);
    }

    public async Task UpdateAsync(CompensationRecommendation recommendation, CancellationToken cancellationToken = default)
    {
        _context.CompensationRecommendations.Update(recommendation);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for budget log operations using Entity Framework Core.
/// </summary>
public class BudgetLogRepository : IBudgetLogRepository
{
    private readonly CompensationDbContext _context;

    public BudgetLogRepository(CompensationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BudgetLog>> GetByBudgetIdAsync(decimal budgetId, CancellationToken cancellationToken = default)
    {
        return await _context.BudgetLogs
            .Where(l => l.BudgetId == budgetId)
            .OrderByDescending(l => l.ModifiedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BudgetLog logEntry, CancellationToken cancellationToken = default)
    {
        await _context.BudgetLogs.AddAsync(logEntry, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Unit of Work implementation for managing multiple repositories and transactions.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly CompensationDbContext _context;
    private IBudgetRepository? _budgetRepository;
    private ICompensationLevelRepository? _compensationLevelRepository;
    private ICompensationPeriodRepository? _compensationPeriodRepository;
    private ICompensationRecommendationRepository? _compensationRecommendationRepository;
    private IBudgetLogRepository? _budgetLogRepository;

    public UnitOfWork(CompensationDbContext context)
    {
        _context = context;
    }

    public IBudgetRepository Budgets =>
        _budgetRepository ??= new BudgetRepository(_context);

    public ICompensationLevelRepository CompensationLevels =>
        _compensationLevelRepository ??= new CompensationLevelRepository(_context);

    public ICompensationPeriodRepository CompensationPeriods =>
        _compensationPeriodRepository ??= new CompensationPeriodRepository(_context);

    public ICompensationRecommendationRepository CompensationRecommendations =>
        _compensationRecommendationRepository ??= new CompensationRecommendationRepository(_context);

    public IBudgetLogRepository BudgetLogs =>
        _budgetLogRepository ??= new BudgetLogRepository(_context);

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
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
