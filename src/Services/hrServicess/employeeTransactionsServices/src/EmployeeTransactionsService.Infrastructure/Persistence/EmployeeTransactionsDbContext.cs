using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Entities;
using EmployeeTransactionsService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTransactionsService.Infrastructure.Persistence;

public sealed class EmployeeTransactionsDbContext(
    DbContextOptions<EmployeeTransactionsDbContext> options,
    IDomainEventDispatcher domainEventDispatcher) : DbContext(options), IUnitOfWork
{
    private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;

    public DbSet<EmployeeMain> Employees => Set<EmployeeMain>();
    public DbSet<EmployeeGrade> EmployeeGrades => Set<EmployeeGrade>();
    public DbSet<EmployeeGradeChange> EmployeeGradeChanges => Set<EmployeeGradeChange>();
    public DbSet<EmployeeProbation> EmployeeProbations => Set<EmployeeProbation>();
    public DbSet<AlertGroup> AlertGroups => Set<AlertGroup>();
    public DbSet<AlertGroupEmployeeMap> AlertGroupEmployeeMaps => Set<AlertGroupEmployeeMap>();
    public DbSet<StationeryItemImage> StationeryItemImages => Set<StationeryItemImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeTransactionsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(entity => entity.DomainEvents).ToList();
        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
            entities.ForEach(static entity => entity.ClearDomainEvents());
        }

        return result;
    }
}