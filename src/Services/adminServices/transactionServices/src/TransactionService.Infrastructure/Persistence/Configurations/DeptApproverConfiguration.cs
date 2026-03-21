namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class DeptApproverConfiguration : IEntityTypeConfiguration<DeptApprover>
{
    public void Configure(EntityTypeBuilder<DeptApprover> builder)
    {
        builder.ToTable("SP_DEPT_APPROVER");
        builder.HasKey(a => new { a.LocationId, a.DeptId, a.EmpSysId });
        builder.Property(a => a.LocationId).HasColumnName("DA_LOCATION_ID");
        builder.Property(a => a.UnitCode)
            .HasColumnName("DA_UNIT_CODE")
            .HasMaxLength(3)
            .HasConversion(u => u.Value, v => new UnitCode(v));
        builder.Property(a => a.DeptId).HasColumnName("DA_DEPT_ID");
        builder.Property(a => a.EmpSysId).HasColumnName("DA_EMP_SYSID");
        builder.Property(a => a.Type)
            .HasColumnName("DA_TYPE")
            .HasMaxLength(1)
            .HasConversion(t => t.Value, v => new ApproverType(v));
        builder.Property(a => a.EffectiveDate).HasColumnName("DA_EFFECTIVE_DATE");
        builder.Property(a => a.ClosureDate).HasColumnName("DA_CLOSURE_DATE");
        builder.Property(a => a.UpdatedBy).HasColumnName("DA_UPDATED_BY");
        builder.Property(a => a.UpdatedOn).HasColumnName("DA_UPDATED_ON");

        builder.Ignore(a => a.DomainEvents);
    }
}
