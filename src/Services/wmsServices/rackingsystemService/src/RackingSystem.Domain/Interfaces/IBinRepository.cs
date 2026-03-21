using RackingSystem.Domain.Entities;

namespace RackingSystem.Domain.Interfaces;

public interface IBinRepository
{
    Task<Bin?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Bin?> GetByBarcodeAsync(string barcode, CancellationToken ct = default);
    Task<IEnumerable<Bin>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Bin>> GetByZoneIdAsync(int zoneId, CancellationToken ct = default);
    Task<IEnumerable<Bin>> GetByShelfIdAsync(int shelfId, CancellationToken ct = default);
    Task<IEnumerable<Bin>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<decimal?> GetBinUtilizationAsync(int binId, CancellationToken ct = default);
    Task AddAsync(Bin bin, CancellationToken ct = default);
    void Update(Bin bin);
    void Remove(Bin bin);
}
