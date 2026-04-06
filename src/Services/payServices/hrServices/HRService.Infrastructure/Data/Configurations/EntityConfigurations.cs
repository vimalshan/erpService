using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HRService.Domain.Entities;

namespace HRService.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_Department");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.DepartmentCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.DepartmentName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedDate).HasDefaultValue(DateTime.UtcNow);
        builder.Property(x => x.ModifiedDate).HasDefaultValue(DateTime.UtcNow);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasIndex(x => x.DepartmentCode).IsUnique();
    }
}

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_Position");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.PositionCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PositionTitle).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasIndex(x => x.PositionCode).IsUnique();

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_Employee");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.EmployeeCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.MiddleName).HasMaxLength(100);
        builder.Property(x => x.Gender).HasMaxLength(10);
        builder.Property(x => x.SSN).HasMaxLength(50);
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.EmploymentType).HasConversion<string>();
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();
            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.OwnsOne(x => x.PhoneNumber)
            .Property(p => p.Value)
            .HasColumnName("PhoneNumber");

        builder.HasIndex(x => x.EmployeeCode).IsUnique();

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Position>()
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_LeaveType");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.LeaveTypeName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
    }
}

public class EmployeeLeaveConfiguration : IEntityTypeConfiguration<EmployeeLeave>
{
    public void Configure(EntityTypeBuilder<EmployeeLeave> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_EmployeeLeave");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Reason).HasMaxLength(500);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<LeaveType>()
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_Shift");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ShiftCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ShiftName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasIndex(x => x.ShiftCode).IsUnique();
    }
}

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_Attendance");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Remarks).HasMaxLength(500);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Shift>()
            .WithMany()
            .HasForeignKey(x => x.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.EmployeeId, x.AttendanceDate }).IsUnique();
    }
}

public class SalaryComponentConfiguration : IEntityTypeConfiguration<SalaryComponent>
{
    public void Configure(EntityTypeBuilder<SalaryComponent> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_SalaryComponent");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.ComponentName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ComponentType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
    }
}

public class EmployeeSalaryConfiguration : IEntityTypeConfiguration<EmployeeSalary>
{
    public void Configure(EntityTypeBuilder<EmployeeSalary> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_EmployeeSalary");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.TotalBaseSalary).HasPrecision(18, 2);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
{
    public void Configure(EntityTypeBuilder<PerformanceReview> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("HR_PerformanceReview");

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Comments).HasMaxLength(2000);
        builder.Property(x => x.Rating).HasPrecision(3, 2);
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
