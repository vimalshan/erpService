using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Data;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
        : base(options)
    {
    }

    public DbSet<DemandMaster> DemandMasters { get; set; } = null!;
    public DbSet<SaaBudget> SaaBudgets { get; set; } = null!;
    public DbSet<SaaPeriod> SaaPeriods { get; set; } = null!;
    public DbSet<SaaLevel> SaaLevels { get; set; } = null!;
    public DbSet<SaaRecommend> SaaRecommends { get; set; } = null!;
    public DbSet<SaaSubmit> SaaSubmits { get; set; } = null!;
    public DbSet<SaaMailTrigger> SaaMailTriggers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureDemandMasterEntity(modelBuilder);
        ConfigureSaaBudgetEntity(modelBuilder);
        ConfigureSaaPeriodEntity(modelBuilder);
        ConfigureSaaLevelEntity(modelBuilder);
        ConfigureSaaRecommendEntity(modelBuilder);
        ConfigureSaaSubmitEntity(modelBuilder);
        ConfigureSaaMailTriggerEntity(modelBuilder);
    }

    private static void ConfigureDemandMasterEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DemandMaster>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DemandType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DepartmentId).IsRequired();
            entity.Property(e => e.DemandDescription).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.RequiredDate).IsRequired();
            entity.Property(e => e.Priority).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DemandStatus).IsRequired().HasMaxLength(1);
            entity.Property(e => e.CreatedBy).IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.ApprovalRemarks).HasMaxLength(2000);
            entity.Property(e => e.CompletionRemarks).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.DemandStatus);
            entity.HasIndex(e => e.DepartmentId);
            entity.HasIndex(e => e.Priority);
        });
    }

    private static void ConfigureSaaBudgetEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaBudget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BusinessId).IsRequired();
            entity.Property(e => e.YearId).IsRequired();
            entity.Property(e => e.BudgetAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.UpdatedBy).IsRequired();
            entity.Property(e => e.UpdatedOn).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.YearId);
            entity.HasIndex(e => new { e.BusinessId, e.YearId }).IsUnique();
        });
    }

    private static void ConfigureSaaPeriodEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaPeriod>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.YearId).IsRequired();
            entity.Property(e => e.QuarterNo).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.Property(e => e.PeriodOpenDate).IsRequired();
            entity.Property(e => e.PeriodCloseDate).IsRequired();
            entity.Property(e => e.FormOpenDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.YearId);
            entity.HasIndex(e => e.Status);
        });
    }

    private static void ConfigureSaaLevelEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LevelDesc).IsRequired().HasMaxLength(200);
            entity.Property(e => e.LevelAmount).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LevelReason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.LevelMin).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.LevelMax).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.LevelEffDate).IsRequired();
            entity.Property(e => e.LevelUpdatedBy).IsRequired();
            entity.Property(e => e.LevelUpdatedOn).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }

    private static void ConfigureSaaRecommendEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaRecommend>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.YearId).IsRequired();
            entity.Property(e => e.PeriodId).IsRequired();
            entity.Property(e => e.EmpSysId).IsRequired();
            entity.Property(e => e.LevelId).IsRequired();
            entity.Property(e => e.CtcAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaximumCap).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.EligibilityAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.RecommendAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InitiativeTaken).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.Results).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.AddRemarks).HasMaxLength(2000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.RecommendBy).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RejectionRemarks).HasMaxLength(2000);
            entity.Property(e => e.FinalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InitiativeLetter).HasMaxLength(4000);
            entity.Property(e => e.ResultsLetter).HasMaxLength(4000);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.PeriodId);
            entity.HasIndex(e => e.EmpSysId);
            entity.HasIndex(e => e.Status);

            entity.HasOne(e => e.Period)
                .WithMany()
                .HasForeignKey(e => e.PeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Level)
                .WithMany()
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSaaSubmitEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaSubmit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PeriodId).IsRequired();
            entity.Property(e => e.BusId).IsRequired();
            entity.Property(e => e.BhrFlag).IsRequired().HasMaxLength(1);
            entity.Property(e => e.ChrFlag).IsRequired().HasMaxLength(1);
            entity.Property(e => e.BhrUpdBy).IsRequired();
            entity.Property(e => e.BhrUpdOn).IsRequired();
            entity.Property(e => e.BhrAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ChrAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.PeriodId);
            entity.HasIndex(e => new { e.PeriodId, e.BusId }).IsUnique();

            entity.HasOne(e => e.Period)
                .WithMany()
                .HasForeignKey(e => e.PeriodId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSaaMailTriggerEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaaMailTrigger>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuarterId).IsRequired();
            entity.Property(e => e.EmpSysId).IsRequired();
            entity.Property(e => e.MailId).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TriggeredBy).IsRequired();
            entity.Property(e => e.TriggeredOn).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.QuarterId);
            entity.HasIndex(e => e.EmpSysId);
        });
    }
}
