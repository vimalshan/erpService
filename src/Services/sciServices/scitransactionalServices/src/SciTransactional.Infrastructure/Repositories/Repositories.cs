using MediatR;
using Microsoft.EntityFrameworkCore;
using SciTransactional.Domain.Common;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;
using SciTransactional.Infrastructure.Persistence;

namespace SciTransactional.Infrastructure.Repositories;

public sealed class NavigationRepository(SciTransactionalDbContext context, IPublisher publisher)
    : INavigationRepository
{
    public async Task<SparshNavigationEntity?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.SparshNavigations.FindAsync([id], ct);

    public async Task<IReadOnlyList<SparshNavigationEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.SparshNavigations.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<SparshNavigationEntity>> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await context.SparshNavigations.Where(n => n.UserId == userId).AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(SparshNavigationEntity entity, CancellationToken ct = default)
        => await context.SparshNavigations.AddAsync(entity, ct);

    public void Update(SparshNavigationEntity entity)
        => context.SparshNavigations.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await SaveAndDispatchAsync(context, publisher, ct);

    internal static async Task<int> SaveAndDispatchAsync(SciTransactionalDbContext ctx, IPublisher pub, CancellationToken ct)
    {
        var entitiesWithEvents = ctx.ChangeTracker.Entries<Entity<long>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();
        var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();
        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        var result = await ctx.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await pub.Publish(domainEvent, ct);

        return result;
    }
}

public sealed class NormsRepository(SciTransactionalDbContext context, IPublisher publisher)
    : INormsRepository
{
    public async Task<NormsMainEntity?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.NormsMain.FindAsync([id], ct);

    public async Task<IReadOnlyList<NormsMainEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.NormsMain.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<NormsMasterEntity>> GetDetailsByNormNoAsync(long normNo, CancellationToken ct = default)
        => await context.NormsMaster.Where(n => n.NormNo == normNo).AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(NormsMainEntity entity, CancellationToken ct = default)
        => await context.NormsMain.AddAsync(entity, ct);

    public async Task AddDetailAsync(NormsMasterEntity entity, CancellationToken ct = default)
        => await context.NormsMaster.AddAsync(entity, ct);

    public void Update(NormsMainEntity entity)
        => context.NormsMain.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await NavigationRepository.SaveAndDispatchAsync(context, publisher, ct);
}

public sealed class AdvanceLicenseRepository(SciTransactionalDbContext context, IPublisher publisher)
    : IAdvanceLicenseRepository
{
    public async Task<AdvanceLicenseEntity?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.AdvanceLicenses.FindAsync([id], ct);

    public async Task<IReadOnlyList<AdvanceLicenseEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.AdvanceLicenses.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(AdvanceLicenseEntity entity, CancellationToken ct = default)
        => await context.AdvanceLicenses.AddAsync(entity, ct);

    public void Update(AdvanceLicenseEntity entity)
        => context.AdvanceLicenses.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await NavigationRepository.SaveAndDispatchAsync(context, publisher, ct);
}

public sealed class AutoMailRepository(SciTransactionalDbContext context)
    : IAutoMailRepository
{
    public async Task<IReadOnlyList<AutoMailStatusEntity>> GetAllStatusAsync(CancellationToken ct = default)
        => await context.AutoMailStatuses.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<AutoMailIdEntity>> GetAllMailIdsAsync(CancellationToken ct = default)
        => await context.AutoMailIds.AsNoTracking().ToListAsync(ct);

    public async Task AddStatusAsync(AutoMailStatusEntity entity, CancellationToken ct = default)
        => await context.AutoMailStatuses.AddAsync(entity, ct);

    public async Task AddMailIdAsync(AutoMailIdEntity entity, CancellationToken ct = default)
        => await context.AutoMailIds.AddAsync(entity, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}

public sealed class OrderMapRepository(SciTransactionalDbContext context)
    : IOrderMapRepository
{
    public async Task<IReadOnlyList<ActualOrderMapEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.ActualOrderMaps.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<ActualOrderMapEntity>> GetByTiedOrderIdAsync(decimal tiedOrderDetailId, CancellationToken ct = default)
        => await context.ActualOrderMaps
            .Where(o => o.TiedOrderDetailId == tiedOrderDetailId)
            .AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(ActualOrderMapEntity entity, CancellationToken ct = default)
        => await context.ActualOrderMaps.AddAsync(entity, ct);

    public void Update(ActualOrderMapEntity entity)
        => context.ActualOrderMaps.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}

public sealed class DirectEntryRepository(SciTransactionalDbContext context)
    : IDirectEntryRepository
{
    public async Task<IReadOnlyList<VehicleDirectEntryEntity>> GetAllAsync(CancellationToken ct = default)
        => await context.VehicleDirectEntries.AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<VehicleDirectEntryEntity>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await context.VehicleDirectEntries
            .Where(d => d.TrackingNumber == trackingNumber)
            .AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(VehicleDirectEntryEntity entity, CancellationToken ct = default)
        => await context.VehicleDirectEntries.AddAsync(entity, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
