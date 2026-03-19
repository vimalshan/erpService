using Microsoft.EntityFrameworkCore;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Interfaces;
using MediatR;

namespace ApprovalGroup.Infrastructure.Persistence;

public class ApprovalGroupDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ApprovalGroupDbContext(DbContextOptions<ApprovalGroupDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ApprovalGroupMaster> ApGroupMast => Set<ApprovalGroupMaster>();
    public DbSet<ApprovalGroupMap> ApGroupMap => Set<ApprovalGroupMap>();
    public DbSet<ApprovalGroupUnitMap> ApGroupUnitMap => Set<ApprovalGroupUnitMap>();
    public DbSet<ApprovalGroupPayBy> ApGroupPayBy => Set<ApprovalGroupPayBy>();
    public DbSet<ApprovalGroupMainCatMap> ApGroupMainCatMap => Set<ApprovalGroupMainCatMap>();
    public DbSet<ApprovalGroupUserMap> ApGroupUserMap => Set<ApprovalGroupUserMap>();
    public DbSet<PullMatrixDetail> PullMatrixDet => Set<PullMatrixDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApprovalGroupDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        return result;
    }
}
