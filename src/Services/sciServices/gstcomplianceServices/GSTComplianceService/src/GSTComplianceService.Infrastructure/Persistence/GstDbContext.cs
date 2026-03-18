using GSTComplianceService.Domain.Common;
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GSTComplianceService.Infrastructure.Persistence;

public class GstDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public GstDbContext(DbContextOptions<GstDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(w => 
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    public DbSet<GstMain> GstMains => Set<GstMain>();
    public DbSet<GstHsnDetail> GstHsnDetails => Set<GstHsnDetail>();
    public DbSet<GstServiceDetail> GstServiceDetails => Set<GstServiceDetail>();
    public DbSet<GstStateRegDetail> GstStateRegDetails => Set<GstStateRegDetail>();
    public DbSet<GstSupplier> GstSuppliers => Set<GstSupplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GstDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEvents(CancellationToken cancellationToken)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
    }
}
