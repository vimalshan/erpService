namespace TransactionService.Infrastructure.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Common;
using TransactionService.Domain.Entities;

public sealed class TransactionDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

    public TransactionDbContext(DbContextOptions<TransactionDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<RequestMain> RequestMains => Set<RequestMain>();
    public DbSet<RequestSub> RequestSubs => Set<RequestSub>();
    public DbSet<OrderMain> OrderMains => Set<OrderMain>();
    public DbSet<OrderSub> OrderSubs => Set<OrderSub>();
    public DbSet<DeptBudget> DeptBudgets => Set<DeptBudget>();
    public DbSet<UnitBudget> UnitBudgets => Set<UnitBudget>();
    public DbSet<DeptApprover> DeptApprovers => Set<DeptApprover>();
    public DbSet<UnitApprover> UnitApprovers => Set<UnitApprover>();
    public DbSet<LocationAdmin> LocationAdmins => Set<LocationAdmin>();
    public DbSet<CategoryDefault> CategoryDefaults => Set<CategoryDefault>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(e => e.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_mediator is not null)
        {
            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var aggregate in aggregates)
            aggregate.ClearDomainEvents();

        return result;
    }
}
