using VendorService.Domain.Entities;

namespace VendorService.Domain.Interfaces;

public interface IVendorRepository
{
    Task<VendorMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<VendorMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<VendorMaster>> GetByStatusAsync(char status, CancellationToken ct = default);
    Task<IEnumerable<VendorMaster>> GetByLocationAsync(long locationId, CancellationToken ct = default);
    Task AddAsync(VendorMaster vendor, CancellationToken ct = default);
    void Update(VendorMaster vendor);
    void Remove(VendorMaster vendor);
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    // Dapper-based stored procedure call
    Task<long> AddUpdateVendorSpAsync(
        long? vendorId, long categoryId, long locationId, string name, string? email,
        string address, long updatedBy, char liveStatus, CancellationToken ct = default);
}
