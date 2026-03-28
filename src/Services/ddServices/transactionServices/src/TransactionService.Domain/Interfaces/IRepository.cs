using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public interface IDemandMasterRepository : IRepository<DemandMaster>
{
    Task<IEnumerable<DemandMaster>> GetByStatusAsync(char status, CancellationToken cancellationToken = default);
    Task<IEnumerable<DemandMaster>> GetByDepartmentAsync(long departmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DemandMaster>> GetByPriorityAsync(string priority, CancellationToken cancellationToken = default);
    Task<int> GetStatusCountAsync(char status, CancellationToken cancellationToken = default);
}

public interface ISaaBudgetRepository : IRepository<SaaBudget>
{
    Task<IEnumerable<SaaBudget>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default);
    Task<SaaBudget?> GetByBusinessAndYearAsync(long businessId, long yearId, CancellationToken cancellationToken = default);
}

public interface ISaaPeriodRepository : IRepository<SaaPeriod>
{
    Task<IEnumerable<SaaPeriod>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default);
    Task<SaaPeriod?> GetOpenPeriodAsync(CancellationToken cancellationToken = default);
}

public interface ISaaLevelRepository : IRepository<SaaLevel>
{
    Task<IEnumerable<SaaLevel>> GetActiveLevelsAsync(CancellationToken cancellationToken = default);
}

public interface ISaaRecommendRepository : IRepository<SaaRecommend>
{
    Task<IEnumerable<SaaRecommend>> GetByPeriodAsync(long periodId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaaRecommend>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaaRecommend>> GetByStatusAsync(long status, CancellationToken cancellationToken = default);
    Task<IEnumerable<SaaRecommend>> GetPendingReviewAsync(CancellationToken cancellationToken = default);
}

public interface ISaaSubmitRepository : IRepository<SaaSubmit>
{
    Task<IEnumerable<SaaSubmit>> GetByPeriodAsync(long periodId, CancellationToken cancellationToken = default);
    Task<SaaSubmit?> GetByPeriodAndBusinessAsync(long periodId, long busId, CancellationToken cancellationToken = default);
}

public interface ISaaMailTriggerRepository : IRepository<SaaMailTrigger>
{
    Task<IEnumerable<SaaMailTrigger>> GetByQuarterAsync(long quarterId, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork : IDisposable
{
    IDemandMasterRepository DemandMasters { get; }
    ISaaBudgetRepository SaaBudgets { get; }
    ISaaPeriodRepository SaaPeriods { get; }
    ISaaLevelRepository SaaLevels { get; }
    ISaaRecommendRepository SaaRecommends { get; }
    ISaaSubmitRepository SaaSubmits { get; }
    ISaaMailTriggerRepository SaaMailTriggers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
