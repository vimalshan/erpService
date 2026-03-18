using Microsoft.EntityFrameworkCore;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.Interfaces;

namespace ObjectiveService.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<ControlPoint> ControlPoints { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<GoalSubGoal> GoalSubGoals { get; set; }
    public DbSet<ControlPointRequest> ControlPointRequests { get; set; }
    public DbSet<ControlPointRequestDetail> ControlPointRequestDetails { get; set; }
    public DbSet<ControlPointRequestApproval> ControlPointRequestApprovals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply decimal(18,0) globally — all decimal fields in this schema are integer-like IDs.
        // Scale 0 is also required for SQL Server IDENTITY columns and ensures FK/PK type consistency.
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(0);
        }

        // Configure ValueGeneratedOnAdd for decimal primary key 'Id' on all entities
        // (EF convention only auto-generates for int/long PKs; decimal needs explicit opt-in)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null &&
                (idProperty.ClrType == typeof(decimal) || idProperty.ClrType == typeof(decimal?)) &&
                idProperty.IsPrimaryKey())
            {
                idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
            }
        }

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasIndex(e => e.EmployeeSysId).IsUnique();
        });

        // ControlPoint configuration
        modelBuilder.Entity<ControlPoint>(entity =>
        {
            entity.ToTable("ControlPoints");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UnitOfMeasurement).IsRequired().HasMaxLength(65);
            entity.Property(e => e.UnitFrom).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UnitTo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Source).IsRequired().HasMaxLength(5);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.HasIndex(e => new { e.EmployeeSysId, e.DDYearId });
        });

        // Goal configuration
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.ToTable("Goals");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.Property(e => e.FormFlag).IsRequired();
            entity.Property(e => e.AppraiserRemarks).HasMaxLength(4000);
            entity.HasMany(g => g.SubGoals)
                .WithOne()
                .HasForeignKey("GoalId")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.UserId, e.PeriodFrom, e.PeriodTo });
        });

        // GoalSubGoal configuration
        modelBuilder.Entity<GoalSubGoal>(entity =>
        {
            entity.ToTable("GoalSubGoals");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(65);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Achievement).HasMaxLength(4000);
            entity.Property(e => e.Difference).HasMaxLength(4000);
            entity.Property(e => e.Remarks).HasMaxLength(4000);
        });

        // ControlPointRequest configuration
        modelBuilder.Entity<ControlPointRequest>(entity =>
        {
            entity.ToTable("ControlPointRequests");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.HasMany(r => r.Details)
                .WithOne()
                .HasForeignKey("ControlPointRequestId")
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(r => r.Approvals)
                .WithOne()
                .HasForeignKey("ControlPointRequestId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ControlPointRequestDetail configuration
        modelBuilder.Entity<ControlPointRequestDetail>(entity =>
        {
            entity.ToTable("ControlPointRequestDetails");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(4000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UnitOfMeasurement).IsRequired().HasMaxLength(65);
            entity.Property(e => e.Source).IsRequired().HasMaxLength(5);
            entity.Property(e => e.AppStatus).HasMaxLength(1);
        });

        // ControlPointRequestApproval configuration
        modelBuilder.Entity<ControlPointRequestApproval>(entity =>
        {
            entity.ToTable("ControlPointRequestApprovals");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(1);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Note: Domain event dispatch would be handled by a message broker/event dispatcher
        // in a production system

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
