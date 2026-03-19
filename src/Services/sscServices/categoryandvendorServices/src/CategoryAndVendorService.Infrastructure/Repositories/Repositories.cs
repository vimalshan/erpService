using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using CategoryAndVendorService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CategoryAndVendorService.Infrastructure.Repositories;

public class MainCategoryRepository : IMainCategoryRepository
{
    private readonly CategoryVendorDbContext _db;
    public MainCategoryRepository(CategoryVendorDbContext db) => _db = db;

    public async Task<MainCategory?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _db.MainCategories.Include(m => m.SubCategories).FirstOrDefaultAsync(m => m.MainCatId == id, ct);

    public async Task<IReadOnlyList<MainCategory>> GetAllAsync(CancellationToken ct = default)
        => await _db.MainCategories.Include(m => m.SubCategories).ToListAsync(ct);

    public async Task AddAsync(MainCategory entity, CancellationToken ct = default)
        => await _db.MainCategories.AddAsync(entity, ct);

    public void Update(MainCategory entity) => _db.MainCategories.Update(entity);
    public void Delete(MainCategory entity) => _db.MainCategories.Remove(entity);
}

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly CategoryVendorDbContext _db;
    public SubCategoryRepository(CategoryVendorDbContext db) => _db = db;

    public async Task<SubCategory?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _db.SubCategories.FirstOrDefaultAsync(s => s.SubCatId == id, ct);

    public async Task<IReadOnlyList<SubCategory>> GetByMainCategoryIdAsync(long mainCatId, CancellationToken ct = default)
        => await _db.SubCategories.Where(s => s.MainCatId == mainCatId).ToListAsync(ct);

    public async Task<IReadOnlyList<SubCategory>> GetAllAsync(CancellationToken ct = default)
        => await _db.SubCategories.ToListAsync(ct);

    public async Task AddAsync(SubCategory entity, CancellationToken ct = default)
        => await _db.SubCategories.AddAsync(entity, ct);

    public void Update(SubCategory entity) => _db.SubCategories.Update(entity);
    public void Delete(SubCategory entity) => _db.SubCategories.Remove(entity);
}

public class VendorDocumentRepository : IVendorDocumentRepository
{
    private readonly CategoryVendorDbContext _db;
    public VendorDocumentRepository(CategoryVendorDbContext db) => _db = db;

    public async Task<VendorDocument?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _db.VendorDocuments.Include(v => v.Files).FirstOrDefaultAsync(v => v.VndDocId == id, ct);

    public async Task<IReadOnlyList<VendorDocument>> GetByVendorIdAsync(long vendorId, CancellationToken ct = default)
        => await _db.VendorDocuments.Include(v => v.Files).Where(v => v.VendorId == vendorId).ToListAsync(ct);

    public async Task<IReadOnlyList<VendorDocument>> GetAllAsync(CancellationToken ct = default)
        => await _db.VendorDocuments.Include(v => v.Files).ToListAsync(ct);

    public async Task AddAsync(VendorDocument entity, CancellationToken ct = default)
        => await _db.VendorDocuments.AddAsync(entity, ct);

    public void Update(VendorDocument entity) => _db.VendorDocuments.Update(entity);
    public void Delete(VendorDocument entity) => _db.VendorDocuments.Remove(entity);
}

public class SupportDocumentRepository : ISupportDocumentRepository
{
    private readonly CategoryVendorDbContext _db;
    public SupportDocumentRepository(CategoryVendorDbContext db) => _db = db;

    public async Task<SupportDocument?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _db.SupportDocuments.Include(s => s.Attachments).FirstOrDefaultAsync(s => s.DocId == id, ct);

    public async Task<IReadOnlyList<SupportDocument>> GetAllAsync(CancellationToken ct = default)
        => await _db.SupportDocuments.Include(s => s.Attachments).ToListAsync(ct);

    public async Task AddAsync(SupportDocument entity, CancellationToken ct = default)
        => await _db.SupportDocuments.AddAsync(entity, ct);

    public void Update(SupportDocument entity) => _db.SupportDocuments.Update(entity);
    public void Delete(SupportDocument entity) => _db.SupportDocuments.Remove(entity);
}

public class SupportDocumentCounterRepository : ISupportDocumentCounterRepository
{
    private readonly CategoryVendorDbContext _db;
    public SupportDocumentCounterRepository(CategoryVendorDbContext db) => _db = db;

    public async Task<SupportDocumentCounter?> GetByBuIdAsync(string buId, CancellationToken ct = default)
        => await _db.SupportDocumentCounters.FirstOrDefaultAsync(c => c.BuId == buId, ct);

    public async Task AddAsync(SupportDocumentCounter entity, CancellationToken ct = default)
        => await _db.SupportDocumentCounters.AddAsync(entity, ct);

    public void Update(SupportDocumentCounter entity) => _db.SupportDocumentCounters.Update(entity);
}
