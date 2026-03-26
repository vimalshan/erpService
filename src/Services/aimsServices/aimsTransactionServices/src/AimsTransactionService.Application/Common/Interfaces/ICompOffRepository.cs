using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Application.Common.Interfaces;

public interface ICompOffRepository
{
    Task<CompOffAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompOffAggregate>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CompOffAggregate compOff, CancellationToken cancellationToken = default);
    void Update(CompOffAggregate compOff);
}
