using OtherService.Domain.Entities;

namespace OtherService.Domain.Interfaces;

public interface ILogDdCatDevDetailRepository
{
    Task<IEnumerable<LogDdCatDevDetail>> GetAllAsync(CancellationToken ct = default);
    Task<LogDdCatDevDetail?> GetByKeyAsync(string appId, decimal appNum, CancellationToken ct = default);
    Task<IEnumerable<LogDdCatDevDetail>> GetByReqNumAsync(decimal reqNum, CancellationToken ct = default);
    Task AddAsync(LogDdCatDevDetail entity, CancellationToken ct = default);
    void Update(LogDdCatDevDetail entity);
    void Delete(LogDdCatDevDetail entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
