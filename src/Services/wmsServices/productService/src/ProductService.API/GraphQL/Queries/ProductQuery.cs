using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.API.GraphQL.Queries;

public class ProductQuery
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<Product> GetProducts([Service] ProductDbContext context)
        => context.Products.Include(p => p.Category).AsNoTracking();

    public async Task<Product?> GetProductById([Service] ProductDbContext context, int productId)
        => await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == productId);

    [UseFiltering]
    [UseSorting]
    public IQueryable<Category> GetCategories([Service] ProductDbContext context)
        => context.Categories.Include(c => c.SubCategories).AsNoTracking();

    public async Task<Category?> GetCategoryById([Service] ProductDbContext context, int categoryId)
        => await context.Categories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.CategoryId == categoryId);
}
