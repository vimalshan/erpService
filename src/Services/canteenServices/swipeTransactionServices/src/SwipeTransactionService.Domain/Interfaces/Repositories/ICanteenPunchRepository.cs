using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Domain.Interfaces.Repositories;

public interface ICanteenPunchRepository
{
    Task<CanteenPunch?> GetByEmployeeAndDateAsync(long empSysId, DateTime date, CancellationToken ct = default);
    Task<IEnumerable<CanteenPunch>> GetByEmployeeAsync(long empSysId, DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(CanteenPunch entity, CancellationToken ct = default);
    Task UpdateAsync(CanteenPunch entity, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
}
