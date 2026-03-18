using MemberService.Domain.Aggregates;
using MemberService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberService.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("MEMBER_MASTER");
        builder.HasKey(m => m.MemberNo);

        builder.Property(m => m.MemberNo).HasColumnName("MEMBER_NO").IsRequired().ValueGeneratedNever();
        builder.Property(m => m.TrustCode).HasColumnName("MEMBER_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(m => m.FpsTrustCode).HasColumnName("MEMBER_FPSTRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(m => m.OpfNo).HasColumnName("MEMBER_OPF_NO").IsRequired();
        builder.Property(m => m.PensionNo).HasColumnName("MEMBER_PENSION_NO").IsRequired();
        builder.Property(m => m.MemberName).HasColumnName("MEMBER_NAME").HasMaxLength(65).IsRequired();
        builder.Property(m => m.FatherName).HasColumnName("MEMBER_FATHERNAME").HasMaxLength(65);
        builder.Property(m => m.EnrollmentDate).HasColumnName("MEMBER_ENR_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(m => m.DateOfJoining).HasColumnName("MEMBER_DOJ").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(m => m.EmployeeType).HasColumnName("MEMBER_EMPLOYEE_TYPE").HasColumnType("CHAR(2)").IsRequired();
        builder.Property(m => m.ClosureDate).HasColumnName("MEMBER_CLOSURE_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(m => m.LeaveDate).HasColumnName("MEMBER_LEAVE_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(m => m.LeaveReason).HasColumnName("MEMBER_LEAVE_REASON").HasMaxLength(200);
        builder.Property(m => m.EnrollUserId).HasColumnName("MEMBER_ENROLL_USER_ID").HasMaxLength(25).IsRequired();
        builder.Property(m => m.EnrollSysId).HasColumnName("MEMBER_ENROLL_SYSID").IsRequired();
        builder.Property(m => m.EnrollDate).HasColumnName("MEMBER_ENROLL_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(m => m.UnitCode).HasColumnName("MEMBER_UNIT_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(m => m.EmployeeNo).HasColumnName("MEMBER_EMP_NUM").IsRequired();
        builder.Property(m => m.EmployeeSysId).HasColumnName("MEMBER_EMP_SYSID").IsRequired();
        builder.Property(m => m.DateOfBirth).HasColumnName("MEMBER_DOB").HasColumnType("DATETIME2(3)");
        builder.Property(m => m.Status)
            .HasColumnName("MEMBER_STATUS")
            .HasColumnType("CHAR(1)")
            .IsRequired()
            .HasConversion(s => ((char)s).ToString(), s => (MemberStatus)s[0]);
        builder.Property(m => m.UpdatedBy).HasColumnName("MEMBER_UPDATED_BY");
        builder.Property(m => m.UpdatedOn).HasColumnName("MEMBER_UPDATED_ON").HasColumnType("DATETIME2(3)");

        builder.HasMany(m => m.Nominees)
            .WithOne()
            .HasForeignKey(n => n.MemberNo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.PayrollRecords)
            .WithOne()
            .HasForeignKey(p => p.MemberNo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Contacts)
            .WithOne()
            .HasForeignKey(c => c.MemberNo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(m => m.DomainEvents);

        builder.HasIndex(m => new { m.TrustCode, m.Status }).HasDatabaseName("IDX_MEMBER_MASTER_TRUST_STATUS");
        builder.HasIndex(m => m.EmployeeSysId).HasDatabaseName("IDX_MEMBER_MASTER_EMP_SYSID");
    }
}
