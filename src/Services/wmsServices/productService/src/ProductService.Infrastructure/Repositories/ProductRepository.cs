using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Common;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories;

public class ProductRepository(ProductDbContext context, IMediator mediator) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id, ct);

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
        => await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Sku == sku, ct);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        => await context.Products.Include(p => p.Category).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default)
        => await context.Products.Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId).AsNoTracking().ToListAsync(ct);

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Add(product);
        await SaveAndDispatchEventsAsync(product, ct);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Update(product);
        await SaveAndDispatchEventsAsync(product, ct);
    }

    public async Task DeleteAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync(ct);
    }

    private async Task SaveAndDispatchEventsAsync(BaseEntity entity, CancellationToken ct)
    {
        var events = entity.DomainEvents.ToList();
        entity.ClearDomainEvents();
        await context.SaveChangesAsync(ct);

        foreach (var domainEvent in events)
            await mediator.Publish(domainEvent, ct);
    }
}
