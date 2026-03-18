namespace HRService.Infrastructure.Repositories;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<T> where T : Domain.Common.AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Domain.Entities.Employee> EmployeeRepository { get; }
    IRepository<Domain.Entities.Department> DepartmentRepository { get; }
    IRepository<Domain.Entities.Position> PositionRepository { get; }
    IRepository<Domain.Entities.EmployeeLeave> LeaveRepository { get; }
    IRepository<Domain.Entities.LeaveType> LeaveTypeRepository { get; }
    IRepository<Domain.Entities.Shift> ShiftRepository { get; }
    IRepository<Domain.Entities.Attendance> AttendanceRepository { get; }
    IRepository<Domain.Entities.SalaryComponent> SalaryComponentRepository { get; }
    IRepository<Domain.Entities.EmployeeSalary> EmployeeSalaryRepository { get; }
    IRepository<Domain.Entities.PerformanceReview> PerformanceReviewRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
