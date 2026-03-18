namespace ReportingService.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IAppraisalRepository Appraisals { get; }
    IRepository<Entities.AppraisalGoal> AppraisalGoals { get; }
    IRepository<Entities.AppraiseePerformance> AppraiseePerformances { get; }
    IRepository<Entities.DDRating> DDRatings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}
