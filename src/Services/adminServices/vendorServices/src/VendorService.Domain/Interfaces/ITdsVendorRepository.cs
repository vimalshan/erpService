using VendorService.Domain.Entities;

namespace VendorService.Domain.Interfaces;

public interface ITdsVendorRepository
{
    Task<IEnumerable<TdsVendor>> GetAllAsync(CancellationToken ct = default);
    Task<TdsVendor?> GetByVendorIdAsync(long vendorId, CancellationToken ct = default);
    Task AddAsync(TdsVendor tdsVendor, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
