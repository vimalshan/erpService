using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class PFAccumulationConfiguration : IEntityTypeConfiguration<PFAccumulation>
{
    public void Configure(EntityTypeBuilder<PFAccumulation> builder)
    {
        builder.ToTable("PF_ACCUMULATION");
        builder.HasKey(e => e.PfAccId);

        builder.Property(e => e.PfAccId).HasColumnName("PF_ACC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYS_ID");
        builder.Property(e => e.MemberNo).HasColumnName("MEMBER_NO");
        builder.Property(e => e.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.PfAccBal).HasColumnName("PF_ACC_BAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfEmpContTotal).HasColumnName("PF_EMP_CONT_TOTAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfErContTotal).HasColumnName("PF_ER_CONT_TOTAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfVolContTotal).HasColumnName("PF_VOL_CONT_TOTAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfAccStatus).HasColumnName("PF_ACC_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (AccumulationStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(AccumulationStatus.Active)
            .HasSentinel(AccumulationStatus.Active);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasPrecision(3);

        builder.HasMany(e => e.Contributions)
            .WithOne()
            .HasForeignKey(c => c.EmpSysId)
            .HasPrincipalKey(e => e.EmpSysId);

        builder.HasMany(e => e.Certificates)
            .WithOne()
            .HasForeignKey(c => c.EmpSysId)
            .HasPrincipalKey(e => e.EmpSysId);

        builder.HasIndex(e => e.EmpSysId).HasDatabaseName("IDX_PF_ACCUMULATION_EMPSYSID").IsUnique();
        builder.HasIndex(e => e.MemberNo).HasDatabaseName("IDX_PF_ACCUMULATION_MEMBERNO");

        builder.Ignore(e => e.DomainEvents);
    }
}
