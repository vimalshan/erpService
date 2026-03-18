using Microsoft.EntityFrameworkCore;
using System;
using EmployeeService.Domain.Common;
using EmployeeService.Domain.Entities;

namespace EmployeeService.Infrastructure.Persistence;

/// <summary>
/// DbContext for Employee Service
/// </summary>
public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<SalaryIncrementLog> SalaryIncrementLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Exclude domain event base class from being mapped
        modelBuilder.Ignore<DomainEvent>();

        // Configure Employee entity
        modelBuilder.Entity<Employee>(builder =>
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EmployeeSystemId)
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.MiddleName)
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.CostCenterId)
                .HasMaxLength(100);

            // Value Objects
            builder.OwnsOne(e => e.GrossCTC, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("GrossCTC")
                    .HasPrecision(19, 2);

                money.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            builder.OwnsOne(e => e.BasicSalary, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("BasicSalary")
                    .HasPrecision(19, 2);

                money.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            builder.Property(e => e.EmploymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(e => e.EmployeeSystemId).IsUnique();
            builder.HasIndex(e => e.Email);
            builder.HasIndex(e => e.EmployeeCode);
            builder.HasIndex(e => e.CostCenterId);
        });

        // Configure SalaryIncrementLog entity
        modelBuilder.Entity<SalaryIncrementLog>(builder =>
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.EmployeeSystemId)
                .IsRequired();

            builder.OwnsOne(l => l.OldCTC, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("OldCTC")
                    .HasPrecision(19, 2);

                money.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            builder.OwnsOne(l => l.NewCTC, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("NewCTC")
                    .HasPrecision(19, 2);

                money.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .HasDefaultValue("INR");
            });

            builder.OwnsOne(l => l.IncrementPercentage, percentage =>
            {
                percentage.Property(p => p.Value)
                    .HasColumnName("IncrementPercentage")
                    .HasPrecision(5, 2);
            });

            builder.Property(l => l.ApprovalComments)
                .HasMaxLength(500);

            builder.Property(l => l.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Approved");

            builder.Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(l => l.EmployeeSystemId);
            builder.HasIndex(l => l.Status);
            builder.HasIndex(l => l.EffectiveDate);
            builder.HasIndex(l => new { l.EmployeeSystemId, l.EffectiveDate });
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
