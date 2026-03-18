using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;
using MasterService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByCodeAsync(string categoryCode, CancellationToken ct = default)
        => await context.Categories.FindAsync([categoryCode.ToUpper()], ct);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        => await context.Categories.OrderBy(c => c.CategoryName).ToListAsync(ct);

    public async Task AddAsync(Category category, CancellationToken ct = default)
    {
        await context.Categories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);
    }
}

public sealed class FinancialYearRepository(ApplicationDbContext context) : IFinancialYearRepository
{
    public async Task<CompanyFinancialYear?> GetBySerialAsync(long serial, CancellationToken ct = default)
        => await context.CompanyFinancialYears.FindAsync([serial], ct);

    public async Task<IEnumerable<CompanyFinancialYear>> GetActiveAsync(CancellationToken ct = default)
        => await context.CompanyFinancialYears
            .Where(f => f.CloseFlag == 'N')
            .OrderByDescending(f => f.StartDate)
            .ToListAsync(ct);

    public async Task AddAsync(CompanyFinancialYear fy, CancellationToken ct = default)
    {
        await context.CompanyFinancialYears.AddAsync(fy, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CompanyFinancialYear fy, CancellationToken ct = default)
    {
        context.CompanyFinancialYears.Update(fy);
        await context.SaveChangesAsync(ct);
    }
}
