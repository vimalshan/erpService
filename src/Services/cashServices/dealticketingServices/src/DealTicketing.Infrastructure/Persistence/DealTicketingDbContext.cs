using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Domain.Common;
using DealTicketing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DealTicketing.Infrastructure.Persistence;

public class DealTicketingDbContext(
    DbContextOptions<DealTicketingDbContext> options,
    IMediator mediator)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<CategoryMaster> CategoryMasters => Set<CategoryMaster>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<DealBatch> DealBatches => Set<DealBatch>();
    public DbSet<DealDetail> DealDetails => Set<DealDetail>();
    public DbSet<DealLoanSchedule> DealLoanSchedules => Set<DealLoanSchedule>();
    public DbSet<DealSettlement> DealSettlements => Set<DealSettlement>();
    public DbSet<DealAttachment> DealAttachments => Set<DealAttachment>();
    public DbSet<DealSettlementAttachment> DealSettlementAttachments => Set<DealSettlementAttachment>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.ApplyConfigurationsFromAssembly(typeof(DealTicketingDbContext).Assembly);
        base.OnModelCreating(model);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var ev in events)
            await mediator.Publish(ev, ct);
    }
}
