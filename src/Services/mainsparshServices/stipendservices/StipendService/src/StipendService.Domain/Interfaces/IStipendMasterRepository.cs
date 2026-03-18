using StipendService.Domain.Entities;

namespace StipendService.Domain.Interfaces;

public interface IStipendMasterRepository
{
    Task<StipendMaster?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StipendMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StipendMaster?> GetActiveByCategory(long categoryId, long rankId, CancellationToken cancellationToken = default);
    Task AddAsync(StipendMaster stipendMaster, CancellationToken cancellationToken = default);
    Task UpdateAsync(StipendMaster stipendMaster, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long categoryId, long rankId, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
