using MediatR;
using MedicineManagement.Domain.Common;
using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Persistence;

public class MedicineManagementDbContext(
    DbContextOptions<MedicineManagementDbContext> options,
    IMediator mediator) : DbContext(options)
{
    public DbSet<MedicineType> MedicineTypes => Set<MedicineType>();
    public DbSet<MedicinePackaging> MedicinePackagings => Set<MedicinePackaging>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<DoctorAttendant> DoctorAttendants => Set<DoctorAttendant>();
    public DbSet<MedicineDebitCreditFlag> MedicineDebitCreditFlags => Set<MedicineDebitCreditFlag>();
    public DbSet<MedicineCredit> MedicineCredits => Set<MedicineCredit>();
    public DbSet<MedicineIssue> MedicineIssues => Set<MedicineIssue>();
    public DbSet<PurchaseMain> PurchaseMains => Set<PurchaseMain>();
    public DbSet<PurchaseSub> PurchaseSubs => Set<PurchaseSub>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicineManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
