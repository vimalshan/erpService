using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Domain.Interfaces.Repositories;

public interface ICanteenDaconRepository
{
    Task<IEnumerable<CanteenDacon>> GetByEmployeeAsync(long empSysId, string date, CancellationToken ct = default);
    Task AddAsync(CanteenDacon entity, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
}
