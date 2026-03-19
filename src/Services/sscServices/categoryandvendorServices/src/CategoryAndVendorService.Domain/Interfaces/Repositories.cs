using CategoryAndVendorService.Domain.Entities;

namespace CategoryAndVendorService.Domain.Interfaces;

public interface IMainCategoryRepository
{
    Task<MainCategory?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<MainCategory>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(MainCategory entity, CancellationToken ct = default);
    void Update(MainCategory entity);
    void Delete(MainCategory entity);
}

public interface ISubCategoryRepository
{
    Task<SubCategory?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<SubCategory>> GetByMainCategoryIdAsync(long mainCatId, CancellationToken ct = default);
    Task<IReadOnlyList<SubCategory>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SubCategory entity, CancellationToken ct = default);
    void Update(SubCategory entity);
    void Delete(SubCategory entity);
}

public interface IVendorDocumentRepository
{
    Task<VendorDocument?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<VendorDocument>> GetByVendorIdAsync(long vendorId, CancellationToken ct = default);
    Task<IReadOnlyList<VendorDocument>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(VendorDocument entity, CancellationToken ct = default);
    void Update(VendorDocument entity);
    void Delete(VendorDocument entity);
}

public interface ISupportDocumentRepository
{
    Task<SupportDocument?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<SupportDocument>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SupportDocument entity, CancellationToken ct = default);
    void Update(SupportDocument entity);
    void Delete(SupportDocument entity);
}

public interface ISupportDocumentCounterRepository
{
    Task<SupportDocumentCounter?> GetByBuIdAsync(string buId, CancellationToken ct = default);
    Task AddAsync(SupportDocumentCounter entity, CancellationToken ct = default);
    void Update(SupportDocumentCounter entity);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
