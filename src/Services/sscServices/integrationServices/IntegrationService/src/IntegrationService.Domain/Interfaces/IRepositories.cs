using IntegrationService.Domain.Entities;

namespace IntegrationService.Domain.Interfaces;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder, long>
{
    Task<PurchaseOrder?> GetByOraclePoIdAsync(long oraclePoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrder>> GetByVendorSiteIdAsync(long vendorSiteId, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetWithMaterialReceiptsAsync(long id, CancellationToken cancellationToken = default);
}

public interface IVendorRepository : IRepository<Vendor, int>
{
    Task<Vendor?> GetByVendorCodeAsync(string vendorCode, CancellationToken cancellationToken = default);
    Task<Vendor?> GetWithSitesAsync(int vendorId, CancellationToken cancellationToken = default);
}

public interface IVendorSiteRepository : IRepository<VendorSite, long>
{
    Task<IEnumerable<VendorSite>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default);
}

public interface IMaterialReceiptRepository : IRepository<MaterialReceiptCertificate, long>
{
    Task<IEnumerable<MaterialReceiptCertificate>> GetByPoIdAsync(long poId, CancellationToken cancellationToken = default);
}

public interface IOrganizationUnitRepository : IRepository<OrganizationUnit, string>
{
    Task<IEnumerable<OrganizationUnit>> GetByBuIdAsync(string buId, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
