using LoanManagement.Domain.Common;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Data;

public class LoanManagementDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public LoanManagementDbContext(DbContextOptions<LoanManagementDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<LoanMain> LoanMain { get; set; }
    public DbSet<LoanDisbursementSchedule> LoanDisbursementSchedules { get; set; }
    public DbSet<LoanInterest> LoanInterests { get; set; }
    public DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();

        // Clear events before publishing to avoid re-publishing on retry
        foreach (var entity in entities)
            entity.ClearDomainEvents();

        // Save changes first
        var result = await base.SaveChangesAsync(cancellationToken);

        // Dispatch domain events after successful save
        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
