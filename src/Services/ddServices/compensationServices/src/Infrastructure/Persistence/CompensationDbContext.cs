namespace CompensationService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CompensationService.Domain;
using CompensationService.Domain.Entities;

/// <summary>
/// The DbContext for the compensation service.
/// </summary>
public class CompensationDbContext : DbContext
{
    public CompensationDbContext(DbContextOptions<CompensationDbContext> options) : base(options) { }

    /// <summary>Gets or sets the budgets.</summary>
    public DbSet<Budget> Budgets { get; set; } = null!;

    /// <summary>Gets or sets the compensation levels.</summary>
    public DbSet<CompensationLevel> CompensationLevels { get; set; } = null!;

    /// <summary>Gets or sets the compensation periods.</summary>
    public DbSet<CompensationPeriod> CompensationPeriods { get; set; } = null!;

    /// <summary>Gets or sets the compensation recommendations.</summary>
    public DbSet<CompensationRecommendation> CompensationRecommendations { get; set; } = null!;

    /// <summary>Gets or sets the budget logs.</summary>
    public DbSet<BudgetLog> BudgetLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore domain events - they're not persisted
        modelBuilder.Ignore<DomainEvent>();

        // Configure Budget
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("BUDGET_ID").HasPrecision(18, 2);
            entity.Property(e => e.BusinessId).HasColumnName("BUSINESS_ID").HasPrecision(18, 2);
            entity.Property(e => e.YearId).HasColumnName("YEAR_ID").HasPrecision(18, 2);
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY").HasPrecision(18, 2);
            entity.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");
            entity.ToTable("SAA_BUDGET");

            // Configure money amount value object
            entity.OwnsOne(e => e.BudgetAmount, b =>
            {
                b.Property(p => p.Amount).HasColumnName("BUDGET_AMOUNT").HasPrecision(18, 2);
            });
        });

        // Configure CompensationLevel
        modelBuilder.Entity<CompensationLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LEVEL_ID").HasPrecision(18, 2);
            entity.Property(e => e.LevelDesc).HasColumnName("LEVEL_DESC").HasMaxLength(10);
            entity.Property(e => e.LevelAmount).HasColumnName("LEVEL_AMOUNT").HasMaxLength(100);
            entity.Property(e => e.LevelReason).HasColumnName("LEVEL_REASON").HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasColumnName("LEVEL_UPDATEDBY").HasPrecision(18, 2);
            entity.Property(e => e.UpdatedOn).HasColumnName("LEVEL_UPDATEDON");
            entity.Property(e => e.EffectiveDate).HasColumnName("LEVEL_EFFDATE");
            entity.Property(e => e.CloseDate).HasColumnName("LEVEL_CLOSEDATE");
            entity.ToTable("SAA_LEVEL");

            // Configure value object
            entity.OwnsOne(e => e.LevelRange, b =>
            {
                b.Property(p => p.MinAmount).HasColumnName("LEVEL_MIN").HasPrecision(18, 2);
                b.Property(p => p.MaxAmount).HasColumnName("LEVEL_MAX").HasPrecision(18, 2);
            });
        });

        // Configure CompensationPeriod
        modelBuilder.Entity<CompensationPeriod>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PERIOD_ID").HasPrecision(18, 2);
            entity.Property(e => e.YearId).HasColumnName("YEAR_ID").HasPrecision(18, 2);
            entity.Property(e => e.QuarterNo).HasColumnName("QUARTER_NO").HasPrecision(18, 2);
            entity.Property(e => e.PeriodOpenDate).HasColumnName("PERIOD_OPENDATE");
            entity.Property(e => e.PeriodCloseDate).HasColumnName("PERIOD_CLOSEDATE");
            entity.Property(e => e.CircularGeneratedOn).HasColumnName("CIRCULAR_GENON");
            entity.Property(e => e.CircularGeneratedBy).HasColumnName("CIRCULAR_GENBY").HasPrecision(18, 2);
            entity.Property(e => e.ReminderLetterOn).HasColumnName("REMINDER_LETON");
            entity.Property(e => e.FormOpenDate).HasColumnName("FORM_OPENDATE");
            entity.Property(e => e.AppraiserLastDate).HasColumnName("APRAISER_LASTDATE");
            entity.Property(e => e.ReviewerLastDate).HasColumnName("REVIEWER_LASTDATE");
            entity.Property(e => e.BhrLastDate).HasColumnName("BHR_LASTDATE");
            entity.Property(e => e.UhrLastDate).HasColumnName("UHR_LASTDATE");
            entity.ToTable("SAA_PERIOD");

            // Configure status value object
            entity.OwnsOne(e => e.Status, b =>
            {
                b.Property(p => p.StatusCode).HasColumnName("STATUS").HasMaxLength(1);
            });
        });

        // Configure CompensationRecommendation
        modelBuilder.Entity<CompensationRecommendation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("RECOMMEND_ID").HasPrecision(18, 2);
            entity.Property(e => e.YearId).HasColumnName("YEAR_ID").HasPrecision(18, 2);
            entity.Property(e => e.PeriodId).HasColumnName("PERIOD_ID").HasPrecision(18, 2);
            entity.Property(e => e.EmployeeSystemId).HasColumnName("EMP_SYSID").HasPrecision(18, 2);
            entity.Property(e => e.LevelId).HasColumnName("LEVEL_ID").HasPrecision(18, 2);
            entity.Property(e => e.InitiativeTaken).HasColumnName("INITIATIVE_TAKEN").HasMaxLength(2000);
            entity.Property(e => e.Results).HasColumnName("RESULTS").HasMaxLength(2000);
            entity.Property(e => e.AdditionalRemarks).HasColumnName("ADD_REMARKS").HasMaxLength(1000);
            entity.Property(e => e.RejectionBy).HasColumnName("REJECTION_BY").HasPrecision(18, 2);
            entity.Property(e => e.RejectionOn).HasColumnName("REJECTION_ON");
            entity.Property(e => e.RejectionRemarks).HasColumnName("REJECTION_REMARKS").HasMaxLength(1000);
            entity.Property(e => e.RecommendedBy).HasColumnName("RECOMMEND_BY").HasMaxLength(3);
            entity.Property(e => e.RecommendSubmittedBy).HasColumnName("RECOMMEND_SUBMITBY").HasPrecision(18, 2);
            entity.Property(e => e.RecommendSubmittedOn).HasColumnName("RECOMMEND_SUBMITON");
            entity.Property(e => e.ReviewerSubmittedBy).HasColumnName("REVIEWER_SUBMITBY").HasPrecision(18, 2);
            entity.Property(e => e.ReviewerSubmittedOn).HasColumnName("REVIEWER_SUBMITON");
            entity.Property(e => e.BhrSubmittedBy).HasColumnName("BHR_SUBMITBY").HasPrecision(18, 2);
            entity.Property(e => e.BhrSubmittedOn).HasColumnName("BHR_SUBMITON");
            entity.Property(e => e.ChrSubmittedBy).HasColumnName("CHR_SUBMITBY").HasPrecision(18, 2);
            entity.Property(e => e.ChrSubmittedOn).HasColumnName("CHR_SUBMITON");
            entity.Property(e => e.UhrSubmittedBy).HasColumnName("UHR_SUBMITBY").HasPrecision(18, 2);
            entity.Property(e => e.UhrSubmittedOn).HasColumnName("UHR_SUBMITON");
            entity.Property(e => e.FinalLevel).HasColumnName("FINAL_LEVEL").HasPrecision(18, 2);
            entity.Property(e => e.FinalAmount).HasColumnName("FINAL_AMOUNT").HasPrecision(18, 2);
            entity.Property(e => e.InitiativeLetter).HasColumnName("INITIATIVE_LETTER").HasMaxLength(2000);
            entity.Property(e => e.ResultsLetter).HasColumnName("RESULTS_LETTER").HasMaxLength(2000);
            entity.ToTable("SAA_RECOMMEND");

            // Configure money amount value objects
            entity.OwnsOne(e => e.CtcAmount, b =>
            {
                b.Property(p => p.Amount).HasColumnName("CTC_AMOUNT").HasPrecision(18, 2);
            });
            entity.OwnsOne(e => e.MaximumCap, b =>
            {
                b.Property(p => p.Amount).HasColumnName("MAXIMUM_CAP").HasPrecision(18, 2);
            });
            entity.OwnsOne(e => e.EligibilityAmount, b =>
            {
                b.Property(p => p.Amount).HasColumnName("ELIGIBILITY_AMOUNT").HasPrecision(18, 2);
            });
            entity.OwnsOne(e => e.RecommendedAmount, b =>
            {
                b.Property(p => p.Amount).HasColumnName("RECOMMEND_AMOUNT").HasPrecision(18, 2);
            });

            // Configure status value object
            entity.OwnsOne(e => e.Status, b =>
            {
                b.Property(p => p.StatusCode).HasColumnName("STATUS");
            });
        });

        // Configure BudgetLog
        modelBuilder.Entity<BudgetLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LOGID").HasPrecision(18, 2);
            entity.Property(e => e.BudgetId).HasColumnName("BUDGETID").HasPrecision(18, 2);
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY").HasPrecision(18, 2);
            entity.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");
            entity.Property(e => e.ModifiedBy).HasColumnName("MOD_BY").HasPrecision(18, 2);
            entity.Property(e => e.ModifiedOn).HasColumnName("MOD_ON");
            entity.ToTable("SAA_BUDGETLOG");

            // Configure money amount value object
            entity.OwnsOne(e => e.BudgetAmount, b =>
            {
                b.Property(p => p.Amount).HasColumnName("BUDGET_AMOUNT").HasPrecision(18, 2);
            });
        });

        // Seed sample data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed data commented out for now - requires proper value object configuration
        // This can be implemented after database is verified
    }
}
