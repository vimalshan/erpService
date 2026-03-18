using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeaveServices.Infrastructure.Persistence;

public sealed class LeaveDbContext : DbContext
{
    private readonly IMediator _mediator;

    public LeaveDbContext(DbContextOptions<LeaveDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<LeaveRequestDetail> LeaveRequestDetails => Set<LeaveRequestDetail>();
    public DbSet<LeaveEncashment> LeaveEncashments => Set<LeaveEncashment>();
    public DbSet<LossOfPay> LossOfPays => Set<LossOfPay>();
    public DbSet<LeaveCounter> LeaveCounters => Set<LeaveCounter>();
    public DbSet<LeaveModel> LeaveModels => Set<LeaveModel>();
    public DbSet<LeaveSignatureId> LeaveSignatureIds => Set<LeaveSignatureId>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaveDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
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
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.DomainEvents)
                await _mediator.Publish(domainEvent, ct);
            entity.ClearDomainEvents();
        }
    }
}
