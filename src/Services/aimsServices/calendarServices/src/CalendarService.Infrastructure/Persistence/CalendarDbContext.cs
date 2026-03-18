using CalendarService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Infrastructure.Persistence;

public class CalendarDbContext(DbContextOptions<CalendarDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<CalendarMaster> CalendarMasters => Set<CalendarMaster>();
    public DbSet<CalendarUnitMap> CalendarUnitMaps => Set<CalendarUnitMap>();
    public DbSet<CalendarRoundRange> CalendarRoundRanges => Set<CalendarRoundRange>();
    public DbSet<CalendarGraceRange> CalendarGraceRanges => Set<CalendarGraceRange>();
    public DbSet<HolidayMaster> HolidayMasters => Set<HolidayMaster>();
    public DbSet<ShiftMaster> ShiftMasters => Set<ShiftMaster>();
    public DbSet<ShiftTimeMaster> ShiftTimeMasters => Set<ShiftTimeMaster>();
    public DbSet<Domain.Entities.ShiftException> ShiftExceptions => Set<Domain.Entities.ShiftException>();
    public DbSet<PatternMaster> PatternMasters => Set<PatternMaster>();
    public DbSet<PatternDetail> PatternDetails => Set<PatternDetail>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CalendarDbContext).Assembly);
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);
        await DispatchDomainEventsAsync(ct);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in events)
            await mediator.Publish(domainEvent, ct);
    }
}
