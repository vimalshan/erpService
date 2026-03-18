using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Domain.Interfaces.Repositories;

public interface IDailyAvailedRepository
{
    Task<DailyAvailed?> GetBySerialAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<DailyAvailed>> GetByEmployeeAsync(long empSysId, string date, CancellationToken ct = default);
    Task AddAsync(DailyAvailed entity, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
}
