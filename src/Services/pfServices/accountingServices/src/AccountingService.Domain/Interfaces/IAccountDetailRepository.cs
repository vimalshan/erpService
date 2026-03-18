using AccountingService.Domain.Entities;

namespace AccountingService.Domain.Interfaces;

public interface IAccountDetailRepository
{
    Task<AccountDetail?> GetByIdAsync(long sysId, CancellationToken ct = default);
    Task<IEnumerable<AccountDetail>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default);
    Task<IEnumerable<AccountDetail>> GetByDateRangeAsync(string trustCode, DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(AccountDetail entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
