namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class UnitApproverConfiguration : IEntityTypeConfiguration<UnitApprover>
{
    public void Configure(EntityTypeBuilder<UnitApprover> builder)
    {
        builder.ToTable("SP_UNIT_APPROVER");
        builder.HasKey(a => new { a.LocationId, a.EmpSysId });
        builder.Property(a => a.LocationId).HasColumnName("UA_LOCATION_ID");
        builder.Property(a => a.UnitCode)
            .HasColumnName("UA_UNIT_CODE")
            .HasMaxLength(3)
            .HasConversion(u => u.Value, v => new UnitCode(v));
        builder.Property(a => a.EmpSysId).HasColumnName("UA_EMP_SYSID");
        builder.Property(a => a.Type)
            .HasColumnName("UA_TYPE")
            .HasMaxLength(1)
            .HasConversion(t => t.Value, v => new ApproverType(v));
        builder.Property(a => a.EffectiveDate).HasColumnName("UA_EFFECTIVE_DATE");
        builder.Property(a => a.ClosureDate).HasColumnName("UA_CLOSURE_DATE").HasMaxLength(255);
        builder.Property(a => a.UpdatedBy).HasColumnName("UA_UPDATED_BY");
        builder.Property(a => a.UpdatedOn).HasColumnName("UA_UPDATED_ON");

        builder.Ignore(a => a.DomainEvents);
    }
}
