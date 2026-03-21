using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.EmployeeId)
            .HasColumnName("employee_id")
            .UseIdentityColumn();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(e => e.EmployeeCode, ec =>
        {
            ec.Property(v => v.Value)
                .HasColumnName("employee_code")
                .HasMaxLength(20)
                .IsRequired();

            ec.HasIndex(v => v.Value).IsUnique();
        });

        builder.Property(e => e.HireDate)
            .HasColumnName("hire_date")
            .IsRequired();

        builder.Property(e => e.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(50);

        builder.Property(e => e.Department)
            .HasColumnName("department")
            .HasMaxLength(50);

        builder.Property(e => e.WarehouseId)
            .HasColumnName("warehouse_id");

        builder.OwnsOne(e => e.Phone, ph =>
        {
            ph.Property(v => v.Value)
                .HasColumnName("phone")
                .HasMaxLength(20);
        });

        builder.OwnsOne(e => e.Email, em =>
        {
            em.Property(v => v.Value)
                .HasColumnName("email")
                .HasMaxLength(100);
        });

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .IsRequired();

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date")
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.UserId).HasDatabaseName("IX_Employee_UserID");
        builder.HasIndex(e => e.WarehouseId).HasDatabaseName("IX_Employee_WarehouseID");
    }
}
