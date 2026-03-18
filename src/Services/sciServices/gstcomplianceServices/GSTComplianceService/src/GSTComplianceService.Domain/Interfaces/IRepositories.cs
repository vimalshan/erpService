using GSTComplianceService.Domain.Entities;

namespace GSTComplianceService.Domain.Interfaces;

public interface IGstMainRepository
{
    Task<GstMain?> GetByIdAsync(long gstId, CancellationToken cancellationToken = default);
    Task<GstMain?> GetByPanNoAsync(string panNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<GstMain>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<GstMain> AddAsync(GstMain gstMain, CancellationToken cancellationToken = default);
    Task UpdateAsync(GstMain gstMain, CancellationToken cancellationToken = default);
    Task DeleteAsync(long gstId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPanNoAsync(string panNo, CancellationToken cancellationToken = default);
}

public interface IGstHsnDetailRepository
{
    Task<IEnumerable<GstHsnDetail>> GetByGstIdAsync(long gstId, CancellationToken cancellationToken = default);
    Task<GstHsnDetail?> GetByIdAsync(long hsnId, CancellationToken cancellationToken = default);
    Task<GstHsnDetail> AddAsync(GstHsnDetail detail, CancellationToken cancellationToken = default);
    Task UpdateAsync(GstHsnDetail detail, CancellationToken cancellationToken = default);
    Task DeleteAsync(long hsnId, CancellationToken cancellationToken = default);
}

public interface IGstStateRegDetailRepository
{
    Task<IEnumerable<GstStateRegDetail>> GetByGstIdAsync(long gstId, CancellationToken cancellationToken = default);
    Task<GstStateRegDetail?> GetByIdAsync(long tinId, CancellationToken cancellationToken = default);
    Task<GstStateRegDetail> AddAsync(GstStateRegDetail detail, CancellationToken cancellationToken = default);
    Task UpdateAsync(GstStateRegDetail detail, CancellationToken cancellationToken = default);
    Task DeleteAsync(long tinId, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
