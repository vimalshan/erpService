using Microsoft.EntityFrameworkCore;
using ReportingService.Domain.Entities;

namespace ReportingService.Infrastructure.Data;

public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appraisal> Appraisals { get; set; } = null!;
    public DbSet<AppraisalGoal> AppraisalGoals { get; set; } = null!;
    public DbSet<AppraiseePerformance> AppraiseePerformances { get; set; } = null!;
    public DbSet<DDRating> DDRatings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAppraisalEntity(modelBuilder);
        ConfigureAppraisalGoalEntity(modelBuilder);
        ConfigureAppraiseePerformanceEntity(modelBuilder);
        ConfigureDDRatingEntity(modelBuilder);
    }

    private static void ConfigureAppraisalEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appraisal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).IsRequired();
            entity.Property(e => e.UserName).HasMaxLength(70);
            entity.Property(e => e.UserId).HasMaxLength(30);
            entity.Property(e => e.StatusDescription).HasMaxLength(100);
            entity.Property(e => e.UnitCode).HasMaxLength(3);
            entity.Property(e => e.GradeCode).HasMaxLength(10);
            entity.Property(e => e.AcademicYear).HasMaxLength(7);
            entity.Property(e => e.DDType).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.RequestNumber);
            entity.HasIndex(e => e.UserId);

            entity.HasMany(e => e.Goals)
                .WithOne()
                .HasForeignKey(g => g.RequestNumber)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Performances)
                .WithOne()
                .HasForeignKey(p => p.RequestNumber)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureAppraisalGoalEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppraisalGoal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.FromUnit).HasMaxLength(20);
            entity.Property(e => e.ToUnit).HasMaxLength(20);
            entity.Property(e => e.AppraiserRemarks).HasMaxLength(4000);
            entity.Property(e => e.CandidateRemarks).HasMaxLength(4000);
            entity.Property(e => e.Achievement).HasMaxLength(4000);
            entity.Property(e => e.Category).HasMaxLength(100);

            entity.HasIndex(e => e.RequestNumber);
        });
    }

    private static void ConfigureAppraiseePerformanceEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppraiseePerformance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).IsRequired();
            entity.Property(e => e.PerformanceSerialNumber).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.PerformanceRemarks).HasMaxLength(4000);
            entity.Property(e => e.MeanRemarks).HasMaxLength(4000);

            entity.HasIndex(e => e.RequestNumber);
        });
    }

    private static void ConfigureDDRatingEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DDRating>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).HasMaxLength(100);
            entity.Property(e => e.BusinessCode).HasMaxLength(100);
            entity.Property(e => e.UnitCode).HasMaxLength(100);
            entity.Property(e => e.BusinessName).HasMaxLength(100);
            entity.Property(e => e.UnitName).HasMaxLength(100);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.BusinessCode);
        });
    }
}
