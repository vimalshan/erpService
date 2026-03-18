using CanteenUnit.Domain.Entities;

namespace CanteenUnit.Domain.Interfaces;

public interface IGenCounterRepository
{
    Task<GenCounter?> GetByTypeAsync(string transType, CancellationToken ct = default);
    Task<long> GetNextNumberAsync(string transType, CancellationToken ct = default);
}
