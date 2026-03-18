using Microsoft.EntityFrameworkCore.Storage;
using HRService.Infrastructure.Data;
using HRService.Domain.Entities;

namespace HRService.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly HRServiceDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Employee>? _employeeRepository;
    private IRepository<Department>? _departmentRepository;
    private IRepository<Position>? _positionRepository;
    private IRepository<EmployeeLeave>? _leaveRepository;
    private IRepository<LeaveType>? _leaveTypeRepository;
    private IRepository<Shift>? _shiftRepository;
    private IRepository<Attendance>? _attendanceRepository;
    private IRepository<SalaryComponent>? _salaryComponentRepository;
    private IRepository<EmployeeSalary>? _employeeSalaryRepository;
    private IRepository<PerformanceReview>? _performanceReviewRepository;

    public UnitOfWork(HRServiceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IRepository<Employee> EmployeeRepository 
        => _employeeRepository ??= new Repository<Employee>(_context);

    public IRepository<Department> DepartmentRepository 
        => _departmentRepository ??= new Repository<Department>(_context);

    public IRepository<Position> PositionRepository 
        => _positionRepository ??= new Repository<Position>(_context);

    public IRepository<EmployeeLeave> LeaveRepository 
        => _leaveRepository ??= new Repository<EmployeeLeave>(_context);

    public IRepository<LeaveType> LeaveTypeRepository 
        => _leaveTypeRepository ??= new Repository<LeaveType>(_context);

    public IRepository<Shift> ShiftRepository 
        => _shiftRepository ??= new Repository<Shift>(_context);

    public IRepository<Attendance> AttendanceRepository 
        => _attendanceRepository ??= new Repository<Attendance>(_context);

    public IRepository<SalaryComponent> SalaryComponentRepository 
        => _salaryComponentRepository ??= new Repository<SalaryComponent>(_context);

    public IRepository<EmployeeSalary> EmployeeSalaryRepository 
        => _employeeSalaryRepository ??= new Repository<EmployeeSalary>(_context);

    public IRepository<PerformanceReview> PerformanceReviewRepository 
        => _performanceReviewRepository ??= new Repository<PerformanceReview>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _transaction?.RollbackAsync(cancellationToken)!;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
