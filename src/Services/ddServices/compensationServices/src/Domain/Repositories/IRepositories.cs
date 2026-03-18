namespace CompensationService.Domain.Repositories;

using CompensationService.Domain.Entities;

/// <summary>
/// Repository interface for budget operations.
/// </summary>
public interface IBudgetRepository
{
    /// <summary>Gets a budget by ID.</summary>
    Task<Budget?> GetByIdAsync(decimal budgetId, CancellationToken cancellationToken = default);

    /// <summary>Gets budgets for a specific year and business.</summary>
    Task<IEnumerable<Budget>> GetByYearAndBusinessAsync(decimal yearId, decimal businessId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new budget.</summary>
    Task AddAsync(Budget budget, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing budget.</summary>
    Task UpdateAsync(Budget budget, CancellationToken cancellationToken = default);

    /// <summary>Saves changes to the repository.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for compensation level operations.
/// </summary>
public interface ICompensationLevelRepository
{
    /// <summary>Gets a level by ID.</summary>
    Task<CompensationLevel?> GetByIdAsync(decimal levelId, CancellationToken cancellationToken = default);

    /// <summary>Gets all active levels.</summary>
    Task<IEnumerable<CompensationLevel>> GetActiveLevelsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets all levels including inactive ones.</summary>
    Task<IEnumerable<CompensationLevel>> GetAllLevelsAsync(CancellationToken cancellationToken = default);

    /// <summary>Adds a new level.</summary>
    Task AddAsync(CompensationLevel level, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing level.</summary>
    Task UpdateAsync(CompensationLevel level, CancellationToken cancellationToken = default);

    /// <summary>Saves changes to the repository.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for compensation period operations.
/// </summary>
public interface ICompensationPeriodRepository
{
    /// <summary>Gets a period by ID.</summary>
    Task<CompensationPeriod?> GetByIdAsync(decimal periodId, CancellationToken cancellationToken = default);

    /// <summary>Gets periods for a specific year.</summary>
    Task<IEnumerable<CompensationPeriod>> GetByYearAsync(decimal yearId, CancellationToken cancellationToken = default);

    /// <summary>Gets a period by year and quarter.</summary>
    Task<CompensationPeriod?> GetByYearAndQuarterAsync(decimal yearId, decimal quarterNo, CancellationToken cancellationToken = default);

    /// <summary>Gets all open periods.</summary>
    Task<IEnumerable<CompensationPeriod>> GetOpenPeriodsAsync(CancellationToken cancellationToken = default);

    /// <summary>Adds a new period.</summary>
    Task AddAsync(CompensationPeriod period, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing period.</summary>
    Task UpdateAsync(CompensationPeriod period, CancellationToken cancellationToken = default);

    /// <summary>Saves changes to the repository.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for compensation recommendation operations.
/// </summary>
public interface ICompensationRecommendationRepository
{
    /// <summary>Gets a recommendation by ID.</summary>
    Task<CompensationRecommendation?> GetByIdAsync(decimal recommendationId, CancellationToken cancellationToken = default);

    /// <summary>Gets recommendations for a specific period and employee.</summary>
    Task<IEnumerable<CompensationRecommendation>> GetByPeriodAndEmployeeAsync(decimal periodId, decimal employeeSystemId, CancellationToken cancellationToken = default);

    /// <summary>Gets all recommendations for a period.</summary>
    Task<IEnumerable<CompensationRecommendation>> GetByPeriodAsync(decimal periodId, CancellationToken cancellationToken = default);

    /// <summary>Gets recommendations by status.</summary>
    Task<IEnumerable<CompensationRecommendation>> GetByStatusAsync(int statusCode, CancellationToken cancellationToken = default);

    /// <summary>Gets pending recommendations for a specific reviewer.</summary>
    Task<IEnumerable<CompensationRecommendation>> GetPendingForReviewerAsync(decimal periodId, string role, CancellationToken cancellationToken = default);

    /// <summary>Adds a new recommendation.</summary>
    Task AddAsync(CompensationRecommendation recommendation, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing recommendation.</summary>
    Task UpdateAsync(CompensationRecommendation recommendation, CancellationToken cancellationToken = default);

    /// <summary>Saves changes to the repository.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for budget log operations.
/// </summary>
public interface IBudgetLogRepository
{
    /// <summary>Gets all logs for a specific budget.</summary>
    Task<IEnumerable<BudgetLog>> GetByBudgetIdAsync(decimal budgetId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new log entry.</summary>
    Task AddAsync(BudgetLog logEntry, CancellationToken cancellationToken = default);

    /// <summary>Saves changes to the repository.</summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of work interface for managing multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>Gets the budget repository.</summary>
    IBudgetRepository Budgets { get; }

    /// <summary>Gets the compensation level repository.</summary>
    ICompensationLevelRepository CompensationLevels { get; }

    /// <summary>Gets the compensation period repository.</summary>
    ICompensationPeriodRepository CompensationPeriods { get; }

    /// <summary>Gets the compensation recommendation repository.</summary>
    ICompensationRecommendationRepository CompensationRecommendations { get; }

    /// <summary>Gets the budget log repository.</summary>
    IBudgetLogRepository BudgetLogs { get; }

    /// <summary>Saves all changes to the database.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>Begins a new transaction.</summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>Commits the current transaction.</summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>Rolls back the current transaction.</summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
