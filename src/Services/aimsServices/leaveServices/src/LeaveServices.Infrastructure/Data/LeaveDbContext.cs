using Microsoft.EntityFrameworkCore;
using LeaveServices.Domain.Entities;
using MediatR;

namespace LeaveServices.Infrastructure.Data;

public sealed class LeaveDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public LeaveDbContext(DbContextOptions<LeaveDbContext> options, IMediator? mediator = null)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<LeaveMaster>          LeaveMasters      { get; set; } = default!;
    public DbSet<LeaveDetails>         LeaveDetails      { get; set; } = default!;
    public DbSet<LeaveCredit>          LeaveCredits      { get; set; } = default!;
    public DbSet<LeaveDetailsApproval> LeaveApprovals    { get; set; } = default!;
    public DbSet<LeaveRules>           LeaveRules        { get; set; } = default!;
    public DbSet<CompOffAdjust>        CompOffAdjustments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaveDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        if (_mediator is null) return;

        var entities = ChangeTracker.Entries<Domain.Common.Entity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var @event in events)
            await _mediator.Publish(@event, ct);
    }
}
