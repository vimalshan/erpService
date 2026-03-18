using CompensationBenefits.Domain.Common;
using CompensationBenefits.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompensationBenefits.Infrastructure.Persistence;

public class CompensationBenefitsDbContext(
    DbContextOptions<CompensationBenefitsDbContext> options,
    IMediator mediator) : DbContext(options)
{
    public DbSet<SalaryMain> SalaryMains => Set<SalaryMain>();
    public DbSet<SalaryDetail> SalaryDetails => Set<SalaryDetail>();
    public DbSet<SalaryStructureMain> SalaryStructureMains => Set<SalaryStructureMain>();
    public DbSet<SalaryStructureDetail> SalaryStructureDetails => Set<SalaryStructureDetail>();
    public DbSet<MediclaimMaster> MediclaimMasters => Set<MediclaimMaster>();
    public DbSet<MediclaimDetail> MediclaimDetails => Set<MediclaimDetail>();
    public DbSet<MediclaimException> MediclaimExceptions => Set<MediclaimException>();
    public DbSet<MediclaimPremiumPercentage> MediclaimPremiumPercentages => Set<MediclaimPremiumPercentage>();
    public DbSet<MediclaimYearlyPremium> MediclaimYearlyPremiums => Set<MediclaimYearlyPremium>();
    public DbSet<MobileConnection> MobileConnections => Set<MobileConnection>();
    public DbSet<MobileLimitMaster> MobileLimitMasters => Set<MobileLimitMaster>();
    public DbSet<MobileAdditionalLimit> MobileAdditionalLimits => Set<MobileAdditionalLimit>();
    public DbSet<EmployeeRetiralEmpSpecific> EmployeeRetiralEmpSpecifics => Set<EmployeeRetiralEmpSpecific>();
    public DbSet<EmployeeRetiralDetail> EmployeeRetiralDetails => Set<EmployeeRetiralDetail>();
    public DbSet<RetiralRangeMaster> RetiralRangeMasters => Set<RetiralRangeMaster>();
    public DbSet<BasicSlabIncrement> BasicSlabIncrements => Set<BasicSlabIncrement>();
    public DbSet<CompensationParameter> CompensationParameters => Set<CompensationParameter>();
    public DbSet<DiligenceRateMaster> DiligenceRateMasters => Set<DiligenceRateMaster>();
    public DbSet<PmsCashPay> PmsCashPays => Set<PmsCashPay>();
    public DbSet<PmsCashPayDetail> PmsCashPayDetails => Set<PmsCashPayDetail>();
    public DbSet<EmployeeCtcRemarks> EmployeeCtcRemarks => Set<EmployeeCtcRemarks>();
    public DbSet<TevCtc> TevCtcs => Set<TevCtc>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompensationBenefitsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var evt in domainEvents)
            await mediator.Publish(evt, ct);

        return result;
    }
}
