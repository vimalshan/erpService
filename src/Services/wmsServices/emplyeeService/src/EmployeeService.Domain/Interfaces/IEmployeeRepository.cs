namespace EmployeeService.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Domain.Entities.Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Employee?> GetByCodeAsync(string employeeCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Domain.Entities.Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Domain.Entities.Employee>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Employee> AddAsync(Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
    Task UpdateAsync(Domain.Entities.Employee employee, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string employeeCode, CancellationToken cancellationToken = default);
}
