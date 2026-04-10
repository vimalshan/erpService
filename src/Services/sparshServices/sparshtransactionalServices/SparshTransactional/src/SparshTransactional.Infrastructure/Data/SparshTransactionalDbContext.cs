using Microsoft.EntityFrameworkCore;
using SparshTransactional.Domain.Common;
using SparshTransactional.Domain.Entities;
using SparshTransactional.Domain.Interfaces;
using MediatR;

namespace SparshTransactional.Infrastructure.Data;

public class SparshTransactionalDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public SparshTransactionalDbContext(DbContextOptions<SparshTransactionalDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ScholarshipMaster> Scholarships => Set<ScholarshipMaster>();
    public DbSet<EligibilityCriteria> EligibilityCriteria => Set<EligibilityCriteria>();
    public DbSet<ScholarshipApplication> Applications => Set<ScholarshipApplication>();
    public DbSet<ScholarshipDisbursement> Disbursements => Set<ScholarshipDisbursement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SparshTransactionalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, ct);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
