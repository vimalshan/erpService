using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Interfaces;

public interface IMainAccountRepository
{
    Task<MainAccount?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<MainAccount>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(MainAccount entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
