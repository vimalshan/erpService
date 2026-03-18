using MasterService.Domain.Entities;

namespace MasterService.Domain.Interfaces;

public interface IJobRepository
{
    Task<JobMaster?> GetByCodeAsync(long jobCode, CancellationToken ct = default);
    Task<IEnumerable<JobMaster>> GetByCategoryAsync(string? categoryCode = null, CancellationToken ct = default);
    Task AddAsync(JobMaster job, CancellationToken ct = default);
    Task UpdateAsync(JobMaster job, CancellationToken ct = default);
}
