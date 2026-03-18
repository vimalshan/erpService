using LovService.Domain.Common;
using LovService.Domain.Interfaces;
using LovService.Infrastructure.Data;
using MediatR;

namespace LovService.Infrastructure.Repositories;

public sealed class UnitOfWork(
    LovDbContext db,
    ILovTypeMastRepository lovTypeMasts,
    ILovMasterRepository lovMasters,
    IProgramLovMastRepository programLovMasts,
    IPublisher publisher) : IUnitOfWork
{
    public ILovTypeMastRepository LovTypeMasts => lovTypeMasts;
    public ILovMasterRepository LovMasters => lovMasters;
    public IProgramLovMastRepository ProgramLovMasts => programLovMasts;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var domainEntities = db.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await db.SaveChangesAsync(ct);

        foreach (var @event in domainEvents)
            await publisher.Publish(@event, ct);

        return result;
    }
}
