using EmployeePrideManagement.Domain.Entities;

namespace EmployeePrideManagement.Domain.Interfaces;

public interface IPrideMomentRepository
{
    Task<MomentPride?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MomentPride>> GetByEmployeeIdAsync(decimal employeeSysId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<MomentPride> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<MomentPride> AddAsync(MomentPride entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(MomentPride entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(decimal id, CancellationToken cancellationToken = default);
}
