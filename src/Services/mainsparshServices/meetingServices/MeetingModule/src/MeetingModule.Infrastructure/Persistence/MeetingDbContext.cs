using MediatR;
using MeetingModule.Domain.Common;
using MeetingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingModule.Infrastructure.Persistence;

public class MeetingDbContext(DbContextOptions<MeetingDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public DbSet<MeetingType> MeetingTypes => Set<MeetingType>();
    public DbSet<MeetingSchedule> MeetingSchedules => Set<MeetingSchedule>();
    public DbSet<PollDetail> PollDetails => Set<PollDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeetingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
