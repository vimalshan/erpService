using EmployeeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace EmployeeService.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core Database Context for Employee Service
    /// </summary>
    public class EmployeeServiceDbContext : DbContext
    {
        public EmployeeServiceDbContext(DbContextOptions<EmployeeServiceDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAppraisal> Appraisals { get; set; }
        public DbSet<AppraisalObjective> AppraisalObjectives { get; set; }
        public DbSet<AppraisalCompetency> AppraisalCompetencies { get; set; }
        public DbSet<EmployeeCareerPlan> CareerPlans { get; set; }
        public DbSet<EmployeeBenefit> Benefits { get; set; }
        public DbSet<EmployeeAccountability> Accountabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(builder =>
            {
                builder.HasKey(e => e.Id);

                builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(e => e.CreatedOn)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                builder.Property(e => e.ModifiedOn)
                    .IsRequired(false);

                builder.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                // Configure owned properties (Value Objects)
                builder.OwnsOne(e => e.PersonalInfo, po =>
                {
                    po.Property(p => p.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(65);
                    po.Property(p => p.MiddleName).HasColumnName("MiddleName").HasMaxLength(65);
                    po.Property(p => p.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(65);
                    po.Property(p => p.DateOfBirth).HasColumnName("DateOfBirth").IsRequired();
                    po.Property(p => p.Gender).HasColumnName("Gender").IsRequired().HasMaxLength(1);
                });

                builder.OwnsOne(e => e.ContactInfo, co =>
                {
                    co.Property(c => c.Email).HasColumnName("Email").IsRequired().HasMaxLength(100);
                    co.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(20);
                    co.Property(c => c.AlternatePhone).HasColumnName("AlternatePhone").HasMaxLength(20);
                });

                builder.OwnsOne(e => e.EmploymentDetails, eo =>
                {
                    eo.Property(e => e.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(20);
                    eo.Property(e => e.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(25);
                    eo.Property(e => e.NickName).HasColumnName("NickName").HasMaxLength(65);
                    eo.Property(e => e.JoiningDate).HasColumnName("JoiningDate").IsRequired();
                    eo.Property(e => e.EffectiveDate).HasColumnName("EffectiveDate").IsRequired();
                    eo.Property(e => e.ConfirmationDate).HasColumnName("ConfirmationDate");
                    eo.Property(e => e.ExitDate).HasColumnName("ExitDate");
                });

                builder.OwnsOne(e => e.GradeInfo, go =>
                {
                    go.Property(g => g.GradeCode).HasColumnName("GradeCode").IsRequired().HasMaxLength(3);
                    go.Property(g => g.GradeName).HasColumnName("GradeName").IsRequired().HasMaxLength(50);
                    go.Property(g => g.GradeId).HasColumnName("GradeId").IsRequired();
                    go.Property(g => g.CadreName).HasColumnName("CadreName").IsRequired().HasMaxLength(20);
                    go.Property(g => g.GradeType).HasColumnName("GradeType").HasMaxLength(3);
                });

                builder.OwnsOne(e => e.OrganizationalAssignment, oa =>
                {
                    oa.Property(o => o.UnitBusinessId).HasColumnName("UnitBusinessId").IsRequired();
                    oa.Property(o => o.UnitOrgId).HasColumnName("UnitOrgId").IsRequired();
                    oa.Property(o => o.UnitCode).HasColumnName("UnitCode").IsRequired().HasMaxLength(3);
                    oa.Property(o => o.Unit).HasColumnName("Unit").IsRequired().HasMaxLength(20);
                    oa.Property(o => o.Designation).HasColumnName("Designation").IsRequired().HasMaxLength(100);
                    oa.Property(o => o.HRRoleId).HasColumnName("HRRoleId").IsRequired().HasMaxLength(3);
                    oa.Property(o => o.CurrentLevelId).HasColumnName("CurrentLevelId");
                });

                builder.OwnsOne(e => e.SalaryInfo, so =>
                {
                    so.Property(s => s.BasicSalary).HasColumnName("BasicSalary").IsRequired().HasPrecision(18, 2);
                    so.Property(s => s.SalaryType).HasColumnName("SalaryType").IsRequired().HasMaxLength(3);
                    so.Property(s => s.CurrentLevel).HasColumnName("CurrentLevel").HasPrecision(18, 2);
                });

                builder.Property(e => e.Status).HasMaxLength(11).HasDefaultValue("ACTIVE");
                builder.Property(e => e.Salutation).HasMaxLength(200).IsRequired(false);
                builder.Property(e => e.TerminationFlag).HasMaxLength(3).IsRequired(false);
                builder.Property(e => e.ProcessType).HasMaxLength(19).IsRequired(false);
                builder.Property(e => e.InclusionStatus).HasMaxLength(40).IsRequired(false);

                // Configure navigation properties
                builder.HasMany(e => e.Accountabilities)
                    .WithOne(a => a.Employee)
                    .HasForeignKey(a => a.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasMany(e => e.Appraisals)
                    .WithOne(a => a.Employee)
                    .HasForeignKey(a => a.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasMany(e => e.CareerPlans)
                    .WithOne(cp => cp.Employee)
                    .HasForeignKey(cp => cp.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasMany(e => e.Benefits)
                    .WithOne(b => b.Employee)
                    .HasForeignKey(b => b.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Create indexes on scalar properties
                builder.HasIndex(e => e.Status);
                builder.HasIndex(e => e.IsTerminated);
                builder.HasIndex(e => e.IsDeleted);
            });

            // Configure EmployeeAppraisal
            modelBuilder.Entity<EmployeeAppraisal>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("DRAFT");
                builder.Property(a => a.AppraisalDate).HasDefaultValueSql("GETUTCDATE()");
                builder.HasMany(a => a.Objectives)
                    .WithOne(o => o.Appraisal)
                    .HasForeignKey(o => o.AppraisalId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasMany(a => a.Competencies)
                    .WithOne(c => c.Appraisal)
                    .HasForeignKey(c => c.AppraisalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure AppraisalObjective
            modelBuilder.Entity<AppraisalObjective>(builder =>
            {
                builder.HasKey(ao => ao.Id);
                builder.Property(ao => ao.TargetValue).HasPrecision(18, 2);
                builder.Property(ao => ao.AchievedValue).HasPrecision(18, 2);
                builder.Property(ao => ao.WeightagePercentage).HasPrecision(5, 2);
            });

            // Configure AppraisalCompetency
            modelBuilder.Entity<AppraisalCompetency>(builder =>
            {
                builder.HasKey(ac => ac.Id);
                builder.Property(ac => ac.RatingScore).HasPrecision(5, 2);
                builder.Property(ac => ac.WeightagePercentage).HasPrecision(5, 2);
            });

            // Configure EmployeeCareerPlan
            modelBuilder.Entity<EmployeeCareerPlan>(builder =>
            {
                builder.HasKey(cp => cp.Id);
                builder.Property(cp => cp.ProposedSuccessorPeriodMonths).HasDefaultValue(0);
                builder.Property(cp => cp.SuspensionPeriodMonths).HasDefaultValue(0);
                builder.Property(cp => cp.Status).HasMaxLength(20).HasDefaultValue("DRAFT");
            });

            // Configure EmployeeBenefit
            modelBuilder.Entity<EmployeeBenefit>(builder =>
            {
                builder.HasKey(b => b.Id);
                builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("ACTIVE");
                builder.Property(b => b.Amount).HasPrecision(18, 2);
            });

            // Configure EmployeeAccountability
            modelBuilder.Entity<EmployeeAccountability>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.Property(a => a.IsClosed).HasDefaultValue(false);
            });

            // Global query filters for soft delete
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
