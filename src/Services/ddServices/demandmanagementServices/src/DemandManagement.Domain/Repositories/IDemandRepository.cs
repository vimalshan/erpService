using DemandManagement.Domain.Entities;

namespace DemandManagement.Domain.Repositories;

public interface IDemandRepository
{
    Task<DemandMaster> GetByIdAsync(long demandId);
    Task<IEnumerable<DemandMaster>> GetAllAsync();
    Task<long> AddAsync(DemandMaster demand);
    Task UpdateAsync(DemandMaster demand);
    Task DeleteAsync(long demandId);
}
