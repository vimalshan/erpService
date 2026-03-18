using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Domain.Repositories;

public interface IScholarshipMainRepository
{
    Task<ScholarshipMain?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ScholarshipMain>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ScholarshipMain>> GetByEmployeeIdAsync(int employeeSysId, CancellationToken cancellationToken = default);
    Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ScholarshipMain scholarship, CancellationToken cancellationToken = default);
    Task UpdateAsync(ScholarshipMain scholarship, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
