using EximManagement.Application.Interfaces;
using EximManagement.Domain.Entities;
using EximManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EximManagement.Infrastructure.Repositories;

public class EximDataFileRepository(EximDbContext db) : IEximDataFileRepository
{
    public Task<EximDataFile?> GetByIdAsync(long fileId, CancellationToken ct = default)
        => db.EximDataFiles.FindAsync([fileId], ct).AsTask();

    public async Task<IEnumerable<EximDataFile>> GetAllAsync(CancellationToken ct = default)
        => await db.EximDataFiles.Where(f => f.DelFlag != "Y").ToListAsync(ct);

    public async Task AddAsync(EximDataFile file, CancellationToken ct = default)
        => await db.EximDataFiles.AddAsync(file, ct);

    public Task UpdateAsync(EximDataFile file, CancellationToken ct = default)
    {
        db.EximDataFiles.Update(file);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<EximDataFile>> GetByTypeAsync(string fileType, CancellationToken ct = default)
        => await db.EximDataFiles
            .Where(f => f.FileType == fileType.ToUpperInvariant() && f.DelFlag != "Y")
            .OrderByDescending(f => f.FileUploadedOn)
            .ToListAsync(ct);
}

public class EximProductRepository(EximDbContext db) : IEximProductRepository
{
    public Task<EximProduct?> GetByIdAsync(long productId, CancellationToken ct = default)
        => db.EximProducts.FindAsync([productId], ct).AsTask();

    public async Task<IEnumerable<EximProduct>> GetAllAsync(CancellationToken ct = default)
        => await db.EximProducts.Where(p => p.Status == 'Y').ToListAsync(ct);

    public async Task AddAsync(EximProduct product, CancellationToken ct = default)
        => await db.EximProducts.AddAsync(product, ct);

    public Task UpdateAsync(EximProduct product, CancellationToken ct = default)
    {
        db.EximProducts.Update(product);
        return Task.CompletedTask;
    }

    public Task<EximProduct?> GetByNameAsync(string productName, CancellationToken ct = default)
        => db.EximProducts.FirstOrDefaultAsync(p => p.ProductName == productName, ct);
}

public class EximProductGroupRepository(EximDbContext db) : IEximProductGroupRepository
{
    public Task<EximProductGroup?> GetByIdAsync(long groupId, CancellationToken ct = default)
        => db.EximProductGroups.FindAsync([groupId], ct).AsTask();

    public async Task<IEnumerable<EximProductGroup>> GetAllAsync(CancellationToken ct = default)
        => await db.EximProductGroups.Where(g => g.Status == 'Y').ToListAsync(ct);

    public async Task AddAsync(EximProductGroup group, CancellationToken ct = default)
        => await db.EximProductGroups.AddAsync(group, ct);

    public Task UpdateAsync(EximProductGroup group, CancellationToken ct = default)
    {
        db.EximProductGroups.Update(group);
        return Task.CompletedTask;
    }
}

public class EximDataExportRepository(EximDbContext db) : IEximDataExportRepository
{
    public Task<EximDataExport?> GetByIdAsync(long dataId, CancellationToken ct = default)
        => db.EximDataExports.FindAsync([dataId], ct).AsTask();

    public async Task<IEnumerable<EximDataExport>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => await db.EximDataExports
            .Where(e => e.EximDate >= from && e.EximDate <= to)
            .OrderByDescending(e => e.EximDate)
            .Take(1000)
            .ToListAsync(ct);

    public async Task<IEnumerable<EximDataExport>> GetByFileIdAsync(long fileId, CancellationToken ct = default)
        => await db.EximDataExports.Where(e => e.FileId == fileId).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<EximDataExport> records, CancellationToken ct = default)
        => await db.EximDataExports.AddRangeAsync(records, ct);
}

public class EximDataImportRepository(EximDbContext db) : IEximDataImportRepository
{
    public Task<EximDataImport?> GetByIdAsync(long dataId, CancellationToken ct = default)
        => db.EximDataImports.FindAsync([dataId], ct).AsTask();

    public async Task<IEnumerable<EximDataImport>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => await db.EximDataImports
            .Where(e => e.EximDate >= from && e.EximDate <= to)
            .OrderByDescending(e => e.EximDate)
            .Take(1000)
            .ToListAsync(ct);

    public async Task<IEnumerable<EximDataImport>> GetByFileIdAsync(long fileId, CancellationToken ct = default)
        => await db.EximDataImports.Where(e => e.FileId == fileId).ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<EximDataImport> records, CancellationToken ct = default)
        => await db.EximDataImports.AddRangeAsync(records, ct);
}
