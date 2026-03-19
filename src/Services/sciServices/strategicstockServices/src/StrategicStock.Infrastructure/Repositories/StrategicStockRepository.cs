using MediatR;
using Microsoft.EntityFrameworkCore;
using StrategicStock.Domain.Common;
using StrategicStock.Domain.Entities;
using StrategicStock.Domain.Interfaces;
using StrategicStock.Infrastructure.Persistence;

namespace StrategicStock.Infrastructure.Repositories;

public sealed class StrategicStockRepository(StrategicStockDbContext context, IPublisher publisher)
    : IStrategicStockRepository
{
    public async Task<StrategicStockEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.StrategicStocks.FindAsync([id], ct);

    public async Task<IReadOnlyList<StrategicStockEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.StrategicStocks.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<StrategicStockEntity>> GetByItemAndCompanyAsync(
        int sciItemId, int companyUnitId, CancellationToken ct = default)
        => await context.StrategicStocks
            .Where(s => s.SciItemId == sciItemId && s.CompanyUnitId == companyUnitId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(StrategicStockEntity entity, CancellationToken ct = default)
        => await context.StrategicStocks.AddAsync(entity, ct);

    public void Update(StrategicStockEntity entity)
        => context.StrategicStocks.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var entitiesWithEvents = context.ChangeTracker.Entries<Entity<int>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        var result = await context.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, ct);
        }

        return result;
    }
}
