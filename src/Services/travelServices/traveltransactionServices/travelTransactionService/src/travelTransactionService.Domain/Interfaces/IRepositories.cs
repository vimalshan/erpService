using travelTransactionService.Domain.Entities;

namespace travelTransactionService.Domain.Interfaces;

public interface IVendorMasterRepository
{
    Task<VendorMaster?> GetByIdAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VendorMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VendorMaster>> GetByCategoryAsync(string categoryType, CancellationToken cancellationToken = default);
    Task AddAsync(VendorMaster vendor, CancellationToken cancellationToken = default);
    Task UpdateAsync(VendorMaster vendor, CancellationToken cancellationToken = default);
    Task DeleteAsync(long vendorId, CancellationToken cancellationToken = default);
}

public interface ITaxMasterRepository
{
    Task<TaxMaster?> GetByTypeAsync(string taxType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaxMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaxMaster>> GetByVendorAsync(long vendorId, CancellationToken cancellationToken = default);
    Task AddAsync(TaxMaster taxMaster, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaxMaster taxMaster, CancellationToken cancellationToken = default);
}

public interface IJaiInterfaceLineRepository
{
    Task<JaiInterfaceLine?> GetByIdAsync(decimal interfaceLineId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JaiInterfaceLine>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JaiInterfaceLine>> GetByBatchIdAsync(decimal batchId, CancellationToken cancellationToken = default);
    Task AddAsync(JaiInterfaceLine line, CancellationToken cancellationToken = default);
    Task UpdateAsync(JaiInterfaceLine line, CancellationToken cancellationToken = default);
}
