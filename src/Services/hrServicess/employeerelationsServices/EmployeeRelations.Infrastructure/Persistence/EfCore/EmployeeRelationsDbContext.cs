using Microsoft.EntityFrameworkCore;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.ValueObjects;
using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Infrastructure.Persistence.EfCore;

public class EmployeeRelationsDbContext : DbContext
{
    public EmployeeRelationsDbContext(DbContextOptions<EmployeeRelationsDbContext> options) : base(options) { }

    public DbSet<DisciplinaryMain> DisciplinaryMains => Set<DisciplinaryMain>();
    public DbSet<DisciplinaryEmp> DisciplinaryEmps => Set<DisciplinaryEmp>();
    public DbSet<DisciplinaryAction> DisciplinaryActions => Set<DisciplinaryAction>();
    public DbSet<EwsMain> EwsMains => Set<EwsMain>();
    public DbSet<EwsAppInput> EwsAppInputs => Set<EwsAppInput>();
    public DbSet<SurveyMaster> SurveyMasters => Set<SurveyMaster>();
    public DbSet<SurveyQuestion> SurveyQuestions => Set<SurveyQuestion>();
    public DbSet<SurveyOption> SurveyOptions => Set<SurveyOption>();
    public DbSet<SurveyResponseMain> SurveyResponseMains => Set<SurveyResponseMain>();
    public DbSet<SurveyResponseDetail> SurveyResponseDetails => Set<SurveyResponseDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeRelationsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private Task DispatchDomainEventsAsync()
    {
        // Domain events are cleared after save; actual dispatch handled by DomainEventDispatcher
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        return Task.CompletedTask;
    }
}
