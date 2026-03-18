using Microsoft.EntityFrameworkCore;
using TrainingDevelopment.Domain.Common;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TrainingDetail> TrainingDetails => Set<TrainingDetail>();
    public DbSet<InstituteMaster> InstituteMasters => Set<InstituteMaster>();
    public DbSet<ProgramLovMaster> ProgramLovMasters => Set<ProgramLovMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            entry.Entity.ClearDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Domain events are dispatched via MediatR in the UnitOfWork
        return result;
    }

    internal IEnumerable<IDomainEvent> GetPendingDomainEvents() =>
        ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();
}
