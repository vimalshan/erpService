using TdsService.Domain.Entities;

namespace TdsService.Domain.Repositories;

public interface ITdsVendorRepository
{
    Task<TdsVendor?> GetByIdAsync(long vendorId, CancellationToken ct = default);
    Task<TdsVendor?> GetByPanAsync(string panNo, CancellationToken ct = default);
    Task<IReadOnlyList<TdsVendor>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<TdsVendor>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(TdsVendor vendor, CancellationToken ct = default);
    void Update(TdsVendor vendor);
    void Remove(TdsVendor vendor);
    Task<bool> ExistsByPanAsync(string panNo, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
