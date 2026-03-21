using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories;

public class CategoryRepository(ProductDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.Categories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.CategoryId == id, ct);

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default)
        => await context.Categories.Include(c => c.SubCategories).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
        => await context.Categories.Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories).AsNoTracking().ToListAsync(ct);

    public async Task<Category> AddAsync(Category category, CancellationToken ct = default)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync(ct);
        return category;
    }

    public async Task UpdateAsync(Category category, CancellationToken ct = default)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Category category, CancellationToken ct = default)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);
    }
}
