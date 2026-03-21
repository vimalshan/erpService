using ReceivingService.Domain.Entities;

namespace ReceivingService.Domain.Interfaces;

public interface IReceivingRepository
{
    Task<Entities.Receiving?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Entities.Receiving?> GetByNumberAsync(string receivingNumber, CancellationToken ct = default);
    Task<IEnumerable<Entities.Receiving>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<Entities.Receiving>> GetByPoIdAsync(int poId, CancellationToken ct = default);
    Task AddAsync(Entities.Receiving receiving, CancellationToken ct = default);
    void Update(Entities.Receiving receiving);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
