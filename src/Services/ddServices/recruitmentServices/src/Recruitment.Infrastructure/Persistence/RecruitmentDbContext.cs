using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using AppApplication = Recruitment.Domain.Entities.Application;

namespace Recruitment.Infrastructure.Persistence;

public class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options)
    {
    }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<AppApplication> Applications { get; set; }
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
    public DbSet<CourseDetail> CourseDetails { get; set; }
    public DbSet<RecruitmentCycle> RecruitmentCycles { get; set; }
    public DbSet<AssessmentParameter> AssessmentParameters { get; set; }
    public DbSet<SteeringCommitteeAssessment> SteeringCommitteeAssessments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Job configuration
        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.JobId).IsRequired();
            entity.Property(e => e.JobDescription).HasMaxLength(200);
            entity.Property(e => e.RoleDetails).HasMaxLength(200);
            entity.Property(e => e.CadreCode).HasMaxLength(3);
            entity.Property(e => e.PrincipalAccount).HasMaxLength(500);
            entity.Property(e => e.JobType).HasMaxLength(200);
            entity.Property(e => e.BusinessCode).HasMaxLength(9);
            entity.Property(e => e.UnitCode).HasMaxLength(10);
        });

        // Application configuration
        modelBuilder.Entity<AppApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApplicationNumber).IsRequired();
            entity.OwnsOne(e => e.ContactInfo, ai =>
            {
                ai.Property(c => c.SparshId)
                    .HasColumnName("AP_SPARSH_ID")
                    .HasMaxLength(25);
                ai.Property(c => c.SparshPin)
                    .HasColumnName("AP_SPARSH_PIN");
            });
            entity.Property(e => e.CurrentJobDesciption).HasMaxLength(200);
            entity.Property(e => e.Achievements).HasMaxLength(4000);
            entity.Property(e => e.ReasonForJoining).HasMaxLength(4000);
            entity.Property(e => e.Strength).HasMaxLength(4000);
            entity.Property(e => e.Awards).HasMaxLength(4000);
            entity.Property(e => e.CrtDocumentPath).HasMaxLength(200);
            entity.Property(e => e.DomainDocumentPath).HasMaxLength(200);
            entity.HasMany(e => e.StatusHistories)
                .WithOne()
                .HasForeignKey(h => h.ApplicationNumber);
            entity.HasMany(e => e.CourseDetails)
                .WithOne()
                .HasForeignKey(c => c.ApplicationNumber);
        });

        // ApplicationStatusHistory configuration
        modelBuilder.Entity<ApplicationStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.UpdatedBy).HasMaxLength(25);
        });

        // CourseDetail configuration
        modelBuilder.Entity<CourseDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CourseTitle).HasMaxLength(200);
            entity.Property(e => e.Duration).HasMaxLength(200);
            entity.Property(e => e.Institute).HasMaxLength(200);
        });

        // RecruitmentCycle configuration
        modelBuilder.Entity<RecruitmentCycle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RecruitmentCycleNo).IsRequired();
        });

        // AssessmentParameter configuration
        modelBuilder.Entity<AssessmentParameter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ParameterName).HasMaxLength(200);
        });

        // SteeringCommitteeAssessment configuration
        modelBuilder.Entity<SteeringCommitteeAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CommitteeMemberId).HasMaxLength(100);
            entity.Property(e => e.ParameterRemarks).HasMaxLength(500);
            entity.Property(e => e.OtherRemarks).HasMaxLength(2000);
        });
    }
}
