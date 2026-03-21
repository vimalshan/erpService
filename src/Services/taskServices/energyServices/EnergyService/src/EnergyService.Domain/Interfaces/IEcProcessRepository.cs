using EnergyService.Domain.Entities;

namespace EnergyService.Domain.Interfaces;

public interface IEcProcessRepository
{
    Task<EcProcess?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<EcProcess>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(EcProcess entity, CancellationToken ct = default);
    void Update(EcProcess entity);
    void Delete(EcProcess entity);
}
