using ErrorLoggingService.Domain.Entities;

namespace ErrorLoggingService.Domain.Repositories;

public interface IErrorLogRepository
{
    Task<ErrorLog?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ErrorLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task AddAsync(ErrorLog errorLog, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
