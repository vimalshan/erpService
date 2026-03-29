using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Interfaces;

public interface ICheckupCardRepository
{
    Task<CheckupCard?> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CheckupCard>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CheckupCard>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CheckupCard entity, CancellationToken cancellationToken = default);
    void Update(CheckupCard entity);
    void Remove(CheckupCard entity);
}
