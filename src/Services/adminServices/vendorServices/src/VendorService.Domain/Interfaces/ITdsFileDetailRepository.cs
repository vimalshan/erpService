using VendorService.Domain.Entities;

namespace VendorService.Domain.Interfaces;

public interface ITdsFileDetailRepository
{
    Task<TdsFileDetail?> GetByIdAsync(long fileId, CancellationToken ct = default);
    Task<IEnumerable<TdsFileDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(TdsFileDetail fileDetail, CancellationToken ct = default);
    void Update(TdsFileDetail fileDetail);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
