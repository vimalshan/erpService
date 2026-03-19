using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.Infrastructure.Persistence.Repositories;

public class VendorRepository(IntegrationDbContext context) : IVendorRepository
{
    public async Task<Vendor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Vendors.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<Vendor>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Vendors.ToListAsync(cancellationToken);

    public async Task AddAsync(Vendor entity, CancellationToken cancellationToken = default)
        => await context.Vendors.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Vendor entity, CancellationToken cancellationToken = default)
    {
        context.Vendors.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) context.Vendors.Remove(entity);
    }

    public async Task<Vendor?> GetByVendorCodeAsync(string vendorCode, CancellationToken cancellationToken = default)
        => await context.Vendors.FirstOrDefaultAsync(v => v.VendorCode == vendorCode, cancellationToken);

    public async Task<Vendor?> GetWithSitesAsync(int vendorId, CancellationToken cancellationToken = default)
        => await context.Vendors
            .Include(v => v.VendorSites)
            .FirstOrDefaultAsync(v => v.Id == vendorId, cancellationToken);
}

public class VendorSiteRepository(IntegrationDbContext context) : IVendorSiteRepository
{
    public async Task<VendorSite?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.VendorSites.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<VendorSite>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.VendorSites.ToListAsync(cancellationToken);

    public async Task AddAsync(VendorSite entity, CancellationToken cancellationToken = default)
        => await context.VendorSites.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(VendorSite entity, CancellationToken cancellationToken = default)
    {
        context.VendorSites.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) context.VendorSites.Remove(entity);
    }

    public async Task<IEnumerable<VendorSite>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default)
        => await context.VendorSites.Where(vs => vs.VendorId == vendorId).ToListAsync(cancellationToken);
}

public class MaterialReceiptRepository(IntegrationDbContext context) : IMaterialReceiptRepository
{
    public async Task<MaterialReceiptCertificate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.MaterialReceipts.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<MaterialReceiptCertificate>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.MaterialReceipts.ToListAsync(cancellationToken);

    public async Task AddAsync(MaterialReceiptCertificate entity, CancellationToken cancellationToken = default)
        => await context.MaterialReceipts.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(MaterialReceiptCertificate entity, CancellationToken cancellationToken = default)
    {
        context.MaterialReceipts.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) context.MaterialReceipts.Remove(entity);
    }

    public async Task<IEnumerable<MaterialReceiptCertificate>> GetByPoIdAsync(long poId, CancellationToken cancellationToken = default)
        => await context.MaterialReceipts.Where(m => m.PurchaseOrderId == poId).ToListAsync(cancellationToken);
}

public class OrganizationUnitRepository(IntegrationDbContext context) : IOrganizationUnitRepository
{
    public async Task<OrganizationUnit?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await context.OrganizationUnits.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<OrganizationUnit>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.OrganizationUnits.ToListAsync(cancellationToken);

    public async Task AddAsync(OrganizationUnit entity, CancellationToken cancellationToken = default)
        => await context.OrganizationUnits.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(OrganizationUnit entity, CancellationToken cancellationToken = default)
    {
        context.OrganizationUnits.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) context.OrganizationUnits.Remove(entity);
    }

    public async Task<IEnumerable<OrganizationUnit>> GetByBuIdAsync(string buId, CancellationToken cancellationToken = default)
        => await context.OrganizationUnits.Where(o => o.BuId == buId).ToListAsync(cancellationToken);
}
