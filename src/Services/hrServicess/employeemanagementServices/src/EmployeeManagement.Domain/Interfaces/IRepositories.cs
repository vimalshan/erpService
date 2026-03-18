using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<Employee?> GetByEmployeeNoAsync(string employeeNo, CancellationToken ct = default);
    Task<IReadOnlyList<Employee>> GetAllAsync(int page, int size, CancellationToken ct = default);
    Task<IReadOnlyList<Employee>> GetByUnitAsync(string unit, CancellationToken ct = default);
    Task AddAsync(Employee employee, CancellationToken ct = default);
    void Update(Employee employee);
    void Remove(Employee employee);
    Task<int> CountAsync(CancellationToken ct = default);
}

public interface IPromotionRepository
{
    Task<EmployeePromotion?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<EmployeePromotion>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task AddAsync(EmployeePromotion promotion, CancellationToken ct = default);
    void Update(EmployeePromotion promotion);
}

public interface ITransferRepository
{
    Task<EmployeeTransfer?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<EmployeeTransfer>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task AddAsync(EmployeeTransfer transfer, CancellationToken ct = default);
    void Update(EmployeeTransfer transfer);
}

public interface IProbationRepository
{
    Task<EmployeeProbation?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<EmployeeProbation?> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default);
    Task<IReadOnlyList<EmployeeProbation>> GetOverdueAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeProbation probation, CancellationToken ct = default);
    void Update(EmployeeProbation probation);
}
