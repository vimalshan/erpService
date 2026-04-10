using Microsoft.EntityFrameworkCore;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Interfaces;
using MediatR;

namespace SSCTransactional.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<AllocationAggregate> Allocations => Set<AllocationAggregate>();
    public DbSet<DefectiveAttachment> DefectiveAttachments => Set<DefectiveAttachment>();
    public DbSet<CorrespondenceAggregate> Correspondences => Set<CorrespondenceAggregate>();
    public DbSet<CorrespondenceAttachment> CorrespondenceAttachments => Set<CorrespondenceAttachment>();
    public DbSet<DocumentApproval> DocumentApprovals => Set<DocumentApproval>();
    public DbSet<RescanDetail> RescanDetails => Set<RescanDetail>();
    public DbSet<RevokeDetail> RevokeDetails => Set<RevokeDetail>();
    public DbSet<DocumentApprover> DocumentApprovers => Set<DocumentApprover>();
    public DbSet<OracleInvoice> OracleInvoices => Set<OracleInvoice>();
    public DbSet<OraclePayment> OraclePayments => Set<OraclePayment>();
    public DbSet<OracleBankDetail> OracleBankDetails => Set<OracleBankDetail>();
    public DbSet<OracleDueDetail> OracleDueDetails => Set<OracleDueDetail>();
    public DbSet<DocumentStatus> DocumentStatuses => Set<DocumentStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<Entity<long>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish((INotification)domainEvent, ct);
    }
}
