using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class LeaveCreditConfiguration : IEntityTypeConfiguration<LeaveCredit>
{
    public void Configure(EntityTypeBuilder<LeaveCredit> builder)
    {
        builder.ToTable("LEAVE_CREDIT");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("LVC_SYSID").ValueGeneratedNever();

        builder.Property(c => c.EmployeeSysId)
            .HasColumnName("LVC_EMPSYSID")
            .IsRequired();

        builder.Property(c => c.LeaveId)
            .HasColumnName("LVC_LEAVEID")
            .IsRequired();

        builder.Property(c => c.CreditDays)
            .HasColumnName("LVC_CREDITDAYS")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(c => c.CreditDate)
            .HasColumnName("LVC_CREDITDATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(c => c.Remarks)
            .HasColumnName("LVC_REMARKS")
            .HasMaxLength(255);

        builder.Property(c => c.CreatedBy)
            .HasColumnName("LVC_CREATEDBY")
            .IsRequired();

        builder.Property(c => c.CreatedOn)
            .HasColumnName("LVC_CREATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(c => c.DomainEvents);

        builder.HasIndex(c => new { c.EmployeeSysId, c.LeaveId }).HasDatabaseName("IX_LEAVE_CREDIT_EMP_LEAVE");
    }
}
